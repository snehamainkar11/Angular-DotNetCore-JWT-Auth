# 🔐 AuthDemoAzure - Azure Functions Web API

A lightweight **Azure Functions Worker** application demonstrating JWT authentication and API setup for the **Angular & .NET Core JWT Auth** project. This is the backend API service that handles authentication requests.

---

## 📋 Overview

`AuthDemoAzure` is a cloud-based API built on **Azure Functions** that integrates with the main Angular-DotNetCore-JWT-Auth authentication system. It provides a serverless approach to hosting JWT-secured API endpoints.

---

## 🛠️ Tech Stack

- **Runtime:** .NET Isolated Worker Model (Azure Functions)
- **Framework:** Microsoft Azure Functions Worker SDK
- **Language:** C#
- **Hosting:** Azure Cloud Functions
- **Architecture:** Serverless, Event-Driven

---

## 📁 Project Structure

```
AuthECAPI/
└── AuthDemoAzure/
    ├── Program.cs          # Application entry point & configuration
    ├── AuthDemoAzure.csproj
    └── local.settings.json # Local development settings
```

---

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 or higher
- Azure Functions Core Tools
- Visual Studio or Visual Studio Code
- Azure subscription (for deployment)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/snehamainkar11/Angular-DotNetCore-JWT-Auth.git
cd Angular-DotNetCore-JWT-Auth/AuthECAPI/AuthDemoAzure
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

---

## ⚙️ Configuration

### Program.cs

The `Program.cs` file configures the Azure Functions application:

```csharp
var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Build().Run();
```

**Key Components:**
- **FunctionsApplication.CreateBuilder()** - Initializes the Azure Functions host
- **ConfigureFunctionsWebApplication()** - Configures web-specific middleware and HTTP routing

### Application Insights (Optional)

Uncomment the lines in `Program.cs` to enable monitoring and diagnostics:

```csharp
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();
```

---

## 🔑 API Endpoints

This project provides authentication-related Azure Functions endpoints. Common endpoints include:

- **POST** `/api/auth/register` - Register new user
- **POST** `/api/auth/login` - User login with JWT token generation
- **POST** `/api/auth/refresh` - Refresh expired JWT tokens
- **GET** `/api/auth/validate` - Validate JWT token

*(Specific endpoints defined in Azure Functions trigger attributes)*

---

## 🔐 Security Features

- **JWT Authentication** - Secure token-based authentication
- **ASP.NET Core Identity** - User management and role-based access
- **HTTPS Only** - Enforced secure communication
- **CORS Support** - Cross-origin requests from Angular frontend

---

## 🚢 Deployment

### Deploy to Azure Functions

1. Publish to Azure:
```bash
func azure functionapp publish <FunctionAppName>
```

2. Verify deployment:
```bash
func azure functionapp list-functions <FunctionAppName>
```

### Environment Variables

Set the following in Azure Function App Settings:

- `AzureWebJobsStorage` - Azure Storage connection string
- `JwtSecret` - JWT signing secret key
- `DatabaseConnection` - SQL Server connection string
- `JwtExpiration` - Token expiration time (in minutes)

---

## 📊 Monitoring

Monitor your functions using:

- **Azure Portal** - Function invocations and errors
- **Application Insights** - Performance metrics and diagnostics *(after enabling)*
- **Azure CLI** - Real-time logs:
  ```bash
  func azure functionapp logstream <FunctionAppName>
  ```

---

## 🤝 Integration

This service integrates with:

- **Angular Frontend** - Located in `AuthECClient/` directory
- **Main API** - Primary authentication logic and database operations
- **SQL Server** - User data and credentials storage

---

## 📝 Project Context

This is part of the **Angular & .NET Core JWT Authentication** system:
- **Frontend:** Angular + TypeScript + Bootstrap
- **Backend API:** .NET Core Web API
- **Serverless API:** Azure Functions (this project)
- **Database:** SQL Server
- **Auth Method:** JWT + ASP.NET Core Identity

---

## 🐛 Troubleshooting

### Build Issues
```bash
# Clear NuGet cache
nuget locals all -clear

# Restore packages
dotnet restore
```

### Runtime Errors
- Ensure `.NET` runtime matches project target framework
- Check Azure Functions Core Tools version compatibility
- Verify environment variables are set correctly

### Deployment Issues
- Verify Azure CLI is authenticated: `az login`
- Check Function App quota and resource limits
- Review Azure Portal function logs for detailed errors

---

## 📚 References

- [Azure Functions Documentation](https://learn.microsoft.com/en-us/azure/azure-functions/)
- [.NET Isolated Worker Model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide)
- [JWT Authentication](https://jwt.io/introduction)
- [Application Insights Setup](https://aka.ms/AAt8mw4)

---

## 📄 License

This project is part of the Angular-DotNetCore-JWT-Auth repository.

---

## 👤 Author

**@snehamainkar11**

---

## 🎯 Contributing

Contributions are welcome! Feel free to:
- Report bugs
- Suggest improvements
- Submit pull requests

---

**Last Updated:** 2026-04-17 15:25:53