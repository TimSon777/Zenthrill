FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Zenthrill.DbMigrator/Zenthrill.DbMigrator.csproj"
WORKDIR "/src/Zenthrill.DbMigrator"
RUN dotnet build "Zenthrill.DbMigrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zenthrill.DbMigrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zenthrill.DbMigrator.dll"]
