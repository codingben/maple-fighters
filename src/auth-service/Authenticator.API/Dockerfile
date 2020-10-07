FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 50050

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Authenticator.API.csproj", "./"]
RUN dotnet restore "Authenticator.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Authenticator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authenticator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authenticator.API.dll"]
