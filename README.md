# RAG QDrant Demo - Superhero Movie Database

A C# console application demonstrating Retrieval-Augmented Generation (RAG) using **QDrant vector database**, **Ollama hosting platform**, and **Microsoft KernelMemory** with real semantic embeddings. 

## 🎬 Features

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

### **Configuration**
```csharp
const string QdrantUrl = "http://localhost:6333";
const string OllamaUrl = "http://localhost:11434";
const string EmbeddingModel = "nomic-embed-text:latest";  // Nomic AI's embedding model
const string ChatModel = "gemma3:1b";                     // Google's chat model
const string IndexName = "movies";
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


Source: https://github.com/RajeevPentyala/rag-qdrant-demo-superhero