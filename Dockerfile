# Use .NET SDK as base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY . .
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore

# Use .NET runtime image for final execution
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/bin/Release/net9.0/ ./

# Define the entry point for the application
CMD ["dotnet", "telegrambot.dll"]
