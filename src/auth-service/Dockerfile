FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

COPY ./Authenticator.API/Authenticator.API.csproj ./Authenticator.API/Authenticator.API.csproj
COPY ./Authenticator.Domain/*.csproj ./Authenticator.Domain/Authenticator.Domain.csproj
COPY ./Authenticator.Infrastructure/*.csproj ./Authenticator.Infrastructure/Authenticator.Infrastructure.csproj
COPY ./maple-fighters-auth-service.sln .

RUN dotnet restore maple-fighters-auth-service.sln

COPY . .

RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Authenticator.API.dll"]