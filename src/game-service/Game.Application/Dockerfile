# Build app
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
WORKDIR /source
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# Copy build artifacts
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Game.Application.dll"]