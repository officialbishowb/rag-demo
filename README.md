# RAG QDrant Demo - Superhero Movie Database

A C# console application demonstrating Retrieval-Augmented Generation (RAG) using **QDrant vector database**, **Ollama hosting platform**, and **Microsoft KernelMemory** with real semantic embeddings. This demo features a kid-friendly superhero and Marvel movie database with intelligent search capabilities.

## 🎬 Features

- **Kid-Friendly Superhero Movie Database**: 10 popular superhero and Marvel movies including Spider-Man, Iron Man, Avengers, and more
- **Real Semantic Embeddings**: Uses Nomic AI's `nomic-embed-text:latest` model for high-quality vector embeddings
- **Vector Similarity Search**: Powered by QDrant for fast and accurate similarity matching
- **Conversational AI**: Uses Google's `gemma3:1b` model for natural language responses
- **KernelMemory Integration**: Leverages Microsoft KernelMemory for streamlined RAG operations
- **Interactive Chat Interface**: Natural language queries about movies, actors, directors, and genres

## 🛠️ Technologies Used

- **.NET 9.0** - Modern C# console application
- **Microsoft KernelMemory** - High-level RAG abstraction layer
- **QDrant** - Vector database for similarity search
- **Ollama** - Local AI model hosting platform
- **Nomic AI nomic-embed-text:latest** - 768-dimensional embedding model
- **Google gemma3:1b** - Lightweight chat model for responses

## 📦 NuGet Packages

```xml
<PackageReference Include="Microsoft.KernelMemory.Core" Version="0.98.250508.3" />
<PackageReference Include="Microsoft.KernelMemory.MemoryDb.Qdrant" Version="0.98.250508.3" />
<PackageReference Include="Microsoft.KernelMemory.AI.Ollama" Version="0.98.250508.3" />
<PackageReference Include="Microsoft.SemanticKernel" Version="1.61.0" />
<PackageReference Include="Qdrant.Client" Version="1.15.0" />
<PackageReference Include="Microsoft.Extensions.VectorData.Abstractions" Version="9.7.0" />
<PackageReference Include="Microsoft.Extensions.AI" Version="9.7.1" />
```

## 🎭 Movie Database

The database includes 10 kid-friendly superhero movies:

1. **Spider-Man: Into the Spider-Verse** (2018) - Animated Marvel spider-verse adventure
2. **The Incredibles** (2004) - Pixar's superhero family film  
3. **Guardians of the Galaxy** (2014) - Fun Marvel space adventure
4. **Iron Man** (2008) - Tony Stark's origin story
5. **Thor** (2011) - Norse god superhero adventure
6. **Captain America: The First Avenger** (2011) - Patriotic superhero origin
7. **The Avengers** (2012) - Epic superhero team-up
8. **Ant-Man** (2015) - Size-changing comedy adventure
9. **Black Panther** (2018) - Inspiring Wakanda superhero story
10. **Captain Marvel** (2019) - Empowering female superhero adventure

Each movie includes detailed information about cast, directors, release dates, and plot summaries.

## 🚀 Prerequisites

### 1. Install QDrant
```bash
# Using Docker (recommended)
docker run -p 6333:6333 -p 6334:6334 qdrant/qdrant

# Or download from: https://qdrant.tech/documentation/guides/installation/
```

### 2. Install Ollama
```bash
# Download from: https://ollama.ai/

# Pull required models
# Nomic AI's embedding model
ollama pull nomic-embed-text:latest

# Google's lightweight chat model  
ollama pull gemma3:1b
```

### 3. Verify Services
- QDrant: http://localhost:6333
- Ollama: http://localhost:11434

## 🏃‍♂️ Running the Application

```bash
# Clone and navigate to project
cd RAGQDrantDemo

# Restore packages and build
dotnet restore
dotnet build

# Run the application
dotnet run
```

## 💬 Example Queries

The application supports natural language queries about your superhero movie database:

### **Movie Information**
- "Who plays Iron Man?"
- "Tell me about The Avengers movie"
- "What year was Black Panther released?"

### **Search by Genre/Theme**
- "What animated superhero movies do I have?"
- "Show me Marvel space movies"
- "Which movies feature teams?"

### **Actor and Director Queries**
- "What movies has Chris Evans been in?"
- "Which movies did Christopher Nolan direct?"
- "Show me movies with Robert Downey Jr."

### **Database Queries**
- "List all my superhero movies"
- "How many Marvel movies do I have?"
- "What movies are good for kids?"

## 🔧 Architecture

### **RAG Pipeline**
1. **Document Import**: Movies imported with KernelMemory into QDrant
2. **Text Chunking**: Automatic chunking for optimal embedding size
3. **Embedding Generation**: Real semantic vectors using Nomic AI's nomic-embed-text model
4. **Vector Storage**: Efficient storage in QDrant with meaningful document IDs
5. **Semantic Search**: Query embeddings matched against movie database using Nomic embeddings
6. **Response Generation**: Contextual answers using Google's gemma3:1b model

### **Key Components**
- `Program.cs` - Main application with KernelMemory setup and chat loop
- `MovieData.cs` - Static superhero movie database with detailed information  
- `Movies.cs` - Movie entity model with vector store attributes

### **Configuration**
```csharp
const string QdrantUrl = "http://localhost:6333";
const string OllamaUrl = "http://localhost:11434";
const string EmbeddingModel = "nomic-embed-text:latest";  // Nomic AI's embedding model
const string ChatModel = "gemma3:1b";                     // Google's chat model
const string IndexName = "movies";
```

## 🎯 Sample Output

```
🎬 Movie RAG Chat Demo with QDrant and Ollama
==================================================
🔧 Initializing KernelMemory with Ollama and QDrant...
🎬 Importing movie data with real embeddings...
  ✅ Imported: Spider-Man: Into the Spider-Verse (ID: movie-spider-man-into-the-spider-verse)
  ✅ Imported: The Incredibles (ID: movie-the-incredibles)
  ✅ Imported: Iron Man (ID: movie-iron-man)
  ... (7 more movies)
🎯 Successfully imported 10 movies with real embeddings!

💬 Chat with your movie database! (type 'exit' to quit)
🚀 Now using REAL Nomic AI embeddings for semantic search!

🎭 You: Who plays Iron Man?
🔍 Searching for: 'Who plays Iron Man?'
🔍 Debug: Found 15 relevant sources from QDrant
🤖 AI: Robert Downey Jr. plays Tony Stark/Iron Man in the 2008 Marvel superhero origin story.

📚 Sources from QDrant:
  • Source Name: content.txt
    Link: movies/movie-iron-man/fc967dd962c1456988457ff3d36d8756
    Partition: Title: Iron Man
Description: After being held captive in an Afghan cave, billionaire engineer Tony...
```

## 🔍 Technical Details

### **Embedding Model (Nomic AI)**
- **Model**: nomic-embed-text:latest
- **Provider**: Nomic AI
- **Dimensions**: 768
- **Context Length**: 2048 tokens
- **Purpose**: High-quality semantic embeddings for movie content
- **Strengths**: Excellent for retrieval tasks and semantic similarity

### **Chat Model (Google)**  
- **Model**: gemma3:1b
- **Provider**: Google
- **Parameters**: 1 billion
- **Max Tokens**: 125,000
- **Purpose**: Natural language understanding and response generation

### **Vector Database**
- **QDrant**: Open-source vector similarity search
- **Similarity**: Cosine similarity for semantic matching
- **Index**: "movies" collection with metadata tags
- **Document IDs**: Meaningful identifiers like "movie-iron-man"

## 🐛 Troubleshooting

### **Common Issues**

1. **"Connection refused" errors**
   - Ensure QDrant is running on port 6333
   - Ensure Ollama is running on port 11434

2. **"Model not found" errors**
   ```bash
   # Pull Nomic AI's embedding model
   ollama pull nomic-embed-text:latest
   
   # Pull Google's chat model
   ollama pull gemma3:1b
   ```

3. **"No relevant sources found"**
   - Try different phrasing or keywords
   - Check if the information exists in the movie database
   - Lower the minRelevance threshold (currently 0.3f)

4. **Slow responses**
   - Embedding generation takes time on first run
   - Subsequent queries should be faster due to caching

## 🎓 Learning Outcomes

This demo demonstrates:
- **Real-world RAG implementation** with production-ready tools
- **Semantic search capabilities** using Nomic AI's state-of-the-art embeddings
- **Local AI deployment** without reliance on cloud services
- **Vector database integration** for scalable similarity search
- **Natural language interaction** with structured data
- **Kid-friendly content curation** and family-safe AI responses
- **Multi-provider AI integration** (Nomic AI for embeddings, Google for chat)

## 📚 Next Steps

- Add more superhero movies to expand the database
- Implement movie rating and review functionality  
- Add image search capabilities for movie posters
- Create a web interface for better user experience
- Add support for multiple languages
- Implement user preferences and recommendations

---

**Built with ❤️ for learning RAG, vector databases, and kid-friendly AI applications!**
