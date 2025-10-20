using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;
using Microsoft.KernelMemory.MemoryDb.Qdrant;
using System.Collections.ObjectModel;
using Qdrant.Client;
const string OllamaUrl = "http://localhost:11434";
const string QdrantUrl = "http://localhost:6333";
const string EmbeddingModel = "nomic-embed-text:latest";
const string ChatModel = "gemma3:1b";
const string IndexName = "dokumente";

Console.WriteLine("RAG-Chat mit PDF-Dokument und QDrant/Ollama");
Console.WriteLine(new string('=', 50));

try
{
    // Konfiguration für Ollama
    var ollamaConfig = new OllamaConfig()
    {
        TextModel = new OllamaModelConfig(ChatModel) { MaxTokenTotal = 125000, Seed = 42 },
        EmbeddingModel = new OllamaModelConfig(EmbeddingModel) { MaxTokenTotal = 2048 },
        Endpoint = OllamaUrl
    };

 
    Console.WriteLine("Initialisiere KernelMemory mit QDrant und Ollama...");
    var memoryBuilder = new KernelMemoryBuilder()
        .WithOllamaTextGeneration(ollamaConfig)
        .WithOllamaTextEmbeddingGeneration(ollamaConfig)
        .WithQdrantMemoryDb(QdrantUrl)
        .WithSearchClientConfig(new SearchClientConfig() { AnswerTokens = 4096 });

    var memory = memoryBuilder.Build(new KernelMemoryBuilderBuildOptions
    {
        AllowMixingVolatileAndPersistentData = true
    });

    await ImportPdfFile(memory);

    await StartChat(memory);
}
catch (Exception ex)
{
    Console.WriteLine($" Fehler: {ex.Message}");
    Console.WriteLine("Bitte sicherstellen, dass QDrant auf Port 6333 und Ollama auf Port 11434 läuft.");
    Console.WriteLine("Außerdem müssen folgende Modelle mit ollama geladen werden:");
    Console.WriteLine($"  ollama pull {EmbeddingModel}");
    Console.WriteLine($"  ollama pull {ChatModel}");
}

async Task ImportPdfFile(IKernelMemory memory)
{
    Console.WriteLine("Bitte Pfad zur PDF-Datei eingeben:");
    var pfad = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(pfad) || !File.Exists(pfad))
    {
        Console.WriteLine("Ungültiger Pfad. Abbruch.");
        return;
    }

    var dokumentId = Path.GetFileNameWithoutExtension(pfad)?.ToLower().Replace(" ", "-");

    Console.WriteLine($"Importiere PDF-Datei: {pfad}");

    await memory.ImportDocumentAsync(
        filePath: pfad!,
        documentId: dokumentId,
        index: IndexName,
        tags: new TagCollection {
            { "typ", "pdf-dokument" },
            { "quelle", "benutzer-upload" }
        });

    Console.WriteLine("PDF erfolgreich importiert und indiziert!");
}

async Task StartChat(IKernelMemory memory)
{
    var chatVerlauf = new ChatHistory();

    Console.WriteLine("\nStelle Fragen zum Inhalt deiner PDF-Datei! (Tippe 'exit' zum Beenden)");
    Console.WriteLine("Beispiel: 'Worum geht es in dem Dokument?' oder 'Fasse den Inhalt zusammen'");
    Console.WriteLine(new string('-', 70));

    while (true)
    {
        Console.Write("\nDu: ");
        var benutzereingabe = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(benutzereingabe) || benutzereingabe.ToLower() == "exit")
        {
            Console.WriteLine("Auf Wiedersehen!");
            break;
        }

        try
        {
            var vollerKontext = chatVerlauf.GetHistoryAsContext() + "\nBenutzer: " + benutzereingabe;
            Console.WriteLine($"Suche nach: '{benutzereingabe}'");

            var antwort = await memory.AskAsync(vollerKontext, index: IndexName, minRelevance: 0.3f);

            Console.WriteLine($"KI: {antwort.Result}");

            chatVerlauf.AddUserMessage(benutzereingabe);
            chatVerlauf.AddAnswerMessage(antwort.Result);

            // Zeige relevante Quellen
            if (antwort.RelevantSources.Any())
            {
                Console.WriteLine("\nRelevante Abschnitte aus der PDF:");
                foreach (var quelle in antwort.RelevantSources.Take(3))
                {
                    Console.WriteLine($"  • Quelle: {quelle.SourceName}");
                    Console.WriteLine($"    Ausschnitt: {quelle.Partitions?.FirstOrDefault()?.Text?[..Math.Min(150, quelle.Partitions.FirstOrDefault()?.Text?.Length ?? 0)] ?? "N/A"}...");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\nKeine relevanten Abschnitte gefunden. Versuche andere Formulierungen.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler bei der Anfrage: {ex.Message}");
        }
    }
}

public class ChatHistory
{
    private readonly Collection<string> _messages = [];

    public void AddUserMessage(string message) => _messages.Add($"Benutzer: {message}");
    public void AddAnswerMessage(string message) => _messages.Add($"Assistent: {message}");

    public string GetHistoryAsContext(int maxMessages = 6) =>
        string.Join("\n", _messages.TakeLast(maxMessages));
}
