# AI-Enhanced Task Management System

A modern, full-stack task management application that demonstrates how AI naturally integrates into traditional business applications. Built with .NET 8, React 18, and powered by local AI (Ollama) for intelligent features that multiply user productivity.

## Features

### Core Task Management
- **Complete CRUD Operations** - Create, read, update, and delete tasks with rich metadata
- **Advanced Filtering & Search** - Filter by status, priority, assignee, category, date ranges
- **User Management** - Role-based access control (Admin, Manager, User)
- **Comments & Activity Timeline** - Collaborate with team members on tasks
- **Dashboard Analytics** - Visual insights into task distribution, overdue items, and team workload

### AI-Powered Intelligence

#### 1. AI-Generated Summaries
- Automatically summarize long task threads with key decisions, blockers, and next steps
- One-click regeneration with AI confidence indicators
- Cached for performance with timestamp tracking

#### 2. Smart Form Autofill
- AI suggests title, priority, and category as you type task descriptions
- Real-time suggestions with confidence scores (High/Medium/Low)
- Debounced to minimize API calls, works seamlessly with 50+ character descriptions

#### 3. Intelligent Validation Messages
- Context-aware error messages that explain WHY validation failed
- AI-powered suggestions to fix issues (e.g., "Similar tasks typically take 3-5 days")
- Falls back gracefully to standard validation when AI is unavailable

#### 4. Natural Language Search
- Search using plain English: "Find all high-priority bugs assigned to me with negative customer feedback"
- AI converts natural language to structured filters
- Shows interpreted query for transparency and editing

#### 5. Sentiment Analysis on Customer Feedback
- Automatic sentiment tagging (Positive/Neutral/Negative/Urgent)
- Auto-escalates priority when negative sentiment + critical keywords detected
- Background processing for non-blocking performance
- Dashboard trends showing sentiment over time

#### 6. AI-Powered Assignment Suggestions
- Recommends best assignee based on expertise, past completions, and current workload
- Top 3 suggestions with reasoning: "John Doe (85% match) - Completed 12 similar tasks, currently 3 active"
- Contextual matching using task category and requirements

## Tech Stack

### Backend
- **.NET 8 Web API** - Modern C# with minimal APIs
- **Entity Framework Core 8** - ORM with PostgreSQL provider
- **PostgreSQL 16** - Robust, open-source relational database
- **JWT Authentication** - Secure token-based auth with refresh tokens
- **Microsoft.SemanticKernel** - AI provider abstraction layer
- **Swagger/OpenAPI** - Interactive API documentation

### Frontend
- **React 18** - Modern UI with hooks and concurrent features
- **TypeScript** - Type-safe development
- **Vite** - Lightning-fast build tool and dev server
- **TailwindCSS** - Utility-first styling with custom AI theme
- **React Query (TanStack)** - Server state management and caching
- **Zustand** - Lightweight client state management
- **React Router v6** - Client-side routing
- **Lucide React** - Beautiful, consistent icons
- **Headless UI** - Accessible component primitives

### AI & Infrastructure
- **Ollama (llama3.2:3b)** - Primary local LLM (free, private, no API key needed)
- **OpenAI API** - Optional upgrade for enhanced AI quality
- **Docker & Docker Compose** - Containerized development and deployment
- **PostgreSQL** - Production-ready database
- **Nginx** - Reverse proxy for frontend

## Prerequisites

- **Docker Desktop** - Version 20.10 or higher
- **Node.js** - Version 20 or higher (for local development)
- **.NET 8 SDK** - Version 8.0 or higher (for local development)
- **Git** - Version control

## Quick Start

### Using Docker (Recommended)

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "AI-Enhanced Task Management System"
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **Download Ollama AI model** (first time only)
   ```bash
   # On Windows (Git Bash or WSL)
   bash setup-ollama.sh

   # Or manually
   docker exec taskmanager-ollama ollama pull llama3.2:3b
   ```

4. **Access the application**
   - **Frontend UI**: http://localhost:3000
   - **Backend API**: http://localhost:5000
   - **API Documentation**: http://localhost:5000/swagger
   - **PostgreSQL**: localhost:5432
   - **Ollama API**: http://localhost:11434

5. **Login with default credentials**
   - Email: `admin@taskmanager.com`
   - Password: `Admin123!`

### Local Development (Without Docker)

<details>
<summary>Click to expand local development setup</summary>

#### Backend Setup

1. **Navigate to backend**
   ```bash
   cd src/EnterpriseTaskManager.API
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Update connection string** in `appsettings.Development.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanager;Username=postgres;Password=yourpassword"
   }
   ```

4. **Run migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the API**
   ```bash
   dotnet run
   ```

#### Frontend Setup

1. **Navigate to frontend**
   ```bash
   cd frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Update environment** in `.env.development`
   ```
   VITE_API_BASE_URL=http://localhost:5000/api
   ```

4. **Run development server**
   ```bash
   npm run dev
   ```

#### Ollama Setup (Local AI)

1. **Install Ollama** from https://ollama.ai
2. **Pull the model**
   ```bash
   ollama pull llama3.2:3b
   ```
3. **Ollama runs on** http://localhost:11434

</details>

## Upgrading to OpenAI (Optional)

For better AI quality, you can switch to OpenAI:

1. **Get an API key** from https://platform.openai.com
2. **Update configuration** in `src/EnterpriseTaskManager.API/appsettings.json`:
   ```json
   "AI": {
     "Provider": "OpenAI",
     "OpenAI": {
       "ApiKey": "sk-your-api-key-here",
       "Model": "gpt-4o-mini",
       "MaxTokens": 2000
     }
   }
   ```
3. **Restart backend**
   ```bash
   docker-compose restart backend
   # Or if running locally: dotnet run
   ```

## Project Structure

```
AI-Enhanced Task Management System/
├── src/
│   └── EnterpriseTaskManager.API/
│       ├── Controllers/          # API endpoints
│       ├── Services/             # Business logic
│       │   └── AI/              # AI provider implementations
│       ├── Models/
│       │   ├── Entities/        # Database models
│       │   └── DTOs/            # Data transfer objects
│       ├── Data/                # EF Core DbContext
│       ├── Interfaces/          # Service contracts
│       ├── Middleware/          # Request pipeline
│       └── Program.cs           # App entry point
├── frontend/
│   └── src/
│       ├── components/          # React components
│       │   ├── tasks/          # Task management UI
│       │   ├── ai/             # AI feature components
│       │   └── common/         # Reusable components
│       ├── hooks/              # Custom React hooks
│       ├── services/           # API client layer
│       ├── store/              # State management
│       ├── types/              # TypeScript definitions
│       └── App.tsx             # Root component
├── tests/
│   └── EnterpriseTaskManager.Tests/
├── docker-compose.yml          # Multi-container orchestration
└── README.md                   # This file
```

## API Documentation

Once the backend is running, visit **http://localhost:5000/swagger** for interactive API documentation with:
- All endpoints organized by controller
- Request/response schemas
- Try-it-out functionality
- Authentication examples

## Key Endpoints

### Authentication
- `POST /api/auth/register` - Create new user account
- `POST /api/auth/login` - Login and receive JWT token
- `POST /api/auth/refresh` - Refresh expired token

### Tasks
- `GET /api/tasks` - List all tasks (with filtering, sorting, pagination)
- `GET /api/tasks/{id}` - Get task details
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

### AI Features
- `POST /api/ai/suggest-fields` - Get AI suggestions for task fields
- `POST /api/tasks/{id}/generate-summary` - Generate AI summary of task thread
- `POST /api/ai/search` - Natural language search
- `POST /api/ai/suggest-assignee` - Get AI assignment recommendations
- `POST /api/ai/analyze-sentiment` - Analyze customer feedback sentiment

## Configuration

### Backend Configuration (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=taskmanager;Username=taskuser;Password=taskpass123"
  },
  "JWT": {
    "SecretKey": "your-super-secret-key-change-in-production-min-32-chars",
    "Issuer": "TaskManagerAPI",
    "Audience": "TaskManagerClient",
    "ExpiryMinutes": 60
  },
  "AI": {
    "Provider": "Ollama",
    "Ollama": {
      "Endpoint": "http://ollama:11434",
      "Model": "llama3.2:3b",
      "Timeout": 60
    },
    "EnableCaching": true,
    "CacheDurationMinutes": 60,
    "RateLimitPerUserPerHour": 100
  }
}
```

### Frontend Configuration (`.env`)

```
VITE_API_BASE_URL=http://localhost:5000/api
VITE_ENABLE_AI_FEATURES=true
```

## Testing

### Backend Tests
```bash
cd tests/EnterpriseTaskManager.Tests
dotnet test
```

### Frontend Tests
```bash
cd frontend
npm run test
```

## Deployment

### Production Build

**Backend**
```bash
cd src/EnterpriseTaskManager.API
dotnet publish -c Release -o publish
```

**Frontend**
```bash
cd frontend
npm run build
# Output in dist/ folder
```

### Docker Production

```bash
docker-compose -f docker-compose.yml up -d
```

## Performance Optimizations

- **AI Response Caching** - Identical requests return cached results
- **Rate Limiting** - 100 AI requests per user per hour (configurable)
- **Debouncing** - Frontend waits 500ms before AI autofill requests
- **Pagination** - Large task lists paginated for performance
- **React Query Caching** - Server data cached for 5 minutes
- **Database Indexing** - Optimized queries on frequently accessed fields

## Security Features

- **JWT Authentication** - Secure token-based authentication
- **Password Hashing** - BCrypt with salt
- **SQL Injection Protection** - Parameterized EF Core queries
- **XSS Protection** - React auto-escapes content
- **CORS Configuration** - Restricted to allowed origins
- **Role-Based Authorization** - Admin/Manager/User permissions
- **Refresh Tokens** - Secure token renewal without re-login

## Troubleshooting

### Ollama model not found
```bash
docker exec taskmanager-ollama ollama pull llama3.2:3b
```

### Database connection failed
Ensure PostgreSQL container is running:
```bash
docker-compose ps
docker-compose up postgres -d
```

### Frontend can't reach backend
Check backend is running on correct port:
```bash
curl http://localhost:5000/api/health
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see LICENSE file for details.

## Acknowledgments

- **Ollama** - For making local LLMs accessible and easy
- **Microsoft** - For .NET 8 and Semantic Kernel
- **Anthropic** - For Claude AI inspiring intelligent UX patterns
- **TailwindCSS** - For beautiful, utility-first styling
- **React Team** - For the amazing React 18 features

## Contact & Support

For questions, issues, or feature requests:
- Open an issue on GitHub
- Email: support@taskmanager.example.com

---

**Built with production-quality code, comprehensive error handling, and extensive documentation to demonstrate modern full-stack + AI integration skills.**
