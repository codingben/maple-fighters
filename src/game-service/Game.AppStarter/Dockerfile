FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Game.AppStarter.csproj", "./"]
RUN dotnet restore "Game.AppStarter.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Game.AppStarter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.AppStarter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.AppStarter.dll"]
