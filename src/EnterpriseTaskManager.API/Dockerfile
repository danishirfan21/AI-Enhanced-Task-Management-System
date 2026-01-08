# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["EnterpriseTaskManager.API/EnterpriseTaskManager.API.csproj", "EnterpriseTaskManager.API/"]
RUN dotnet restore "EnterpriseTaskManager.API/EnterpriseTaskManager.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/EnterpriseTaskManager.API"
RUN dotnet build "EnterpriseTaskManager.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "EnterpriseTaskManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnterpriseTaskManager.API.dll"]
