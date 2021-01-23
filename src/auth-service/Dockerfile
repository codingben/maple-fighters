# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# copy csproj and restore as distinct layers
COPY ./Authenticator.API/Authenticator.API.csproj ./Authenticator.API/Authenticator.API.csproj
COPY ./Authenticator.Domain/*.csproj ./Authenticator.Domain/Authenticator.Domain.csproj
COPY ./Authenticator.Infrastructure/*.csproj ./Authenticator.Infrastructure/Authenticator.Infrastructure.csproj
COPY ./maple-fighters-auth-service.sln .
RUN dotnet restore maple-fighters-auth-service.sln

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Authenticator.API.dll"]