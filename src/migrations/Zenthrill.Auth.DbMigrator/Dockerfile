FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["migrations/Zenthrill.Auth.DbMigrator/Zenthrill.Auth.DbMigrator.csproj", "migrations/Zenthrill.Auth.DbMigrator/"]
COPY ["backend/Zenthrill.Auth.Model/Zenthrill.Auth.Model.csproj", "backend/Zenthrill.Auth.Model/"]
RUN dotnet restore "migrations/Zenthrill.Auth.DbMigrator/Zenthrill.Auth.DbMigrator.csproj"
COPY . .
WORKDIR "/src/migrations/Zenthrill.Auth.DbMigrator"
RUN dotnet build "Zenthrill.Auth.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zenthrill.Auth.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zenthrill.Auth.DbMigrator.dll"]
