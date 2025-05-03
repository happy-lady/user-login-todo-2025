# --- Stage 1: Build the .NET backend ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .

# Restore dependencies and build the app
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# --- Stage 2: Run the migration command ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration

WORKDIR /src
COPY --from=build /app/publish /app/publish

# Run migrations
ENTRYPOINT ["dotnet", "ef", "database", "update"]

# --- Stage 3: Final runtime image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app/publish /app

EXPOSE 80

ENTRYPOINT ["dotnet", "MyApp.dll"]