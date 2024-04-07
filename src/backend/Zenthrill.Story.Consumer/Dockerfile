FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/Zenthrill.Story.Consumer/Zenthrill.Story.Consumer.csproj", "src/backend/Zenthrill.Story.Consumer/"]
COPY ["src/backend-libs/Zenthrill.Outbox.Consumer/Zenthrill.Outbox.Consumer.csproj", "src/backend-libs/Zenthrill.Outbox.Consumer/"]
COPY ["src/backend-libs/Zenthrill.Outbox.Core/Zenthrill.Outbox.Core.csproj", "src/backend-libs/Zenthrill.Outbox.Core/"]
COPY ["src/backend-libs/Zenthrill.Settings.DependencyInjection/Zenthrill.Settings.DependencyInjection.csproj", "src/backend-libs/Zenthrill.Settings.DependencyInjection/"]
COPY ["src/backend-libs/Zenthrill.Settings/Zenthrill.Settings.csproj", "src/backend-libs/Zenthrill.Settings/"]
COPY ["src/backend/Zenthrill.Infrastructure/Zenthrill.Infrastructure.csproj", "src/backend/Zenthrill.Infrastructure/"]
COPY ["src/backend-libs/Zenthrill.Providers.DependencyInjection/Zenthrill.Providers.DependencyInjection.csproj", "src/backend-libs/Zenthrill.Providers.DependencyInjection/"]
COPY ["src/backend-libs/Zenthrill.Providers/Zenthrill.Providers.csproj", "src/backend-libs/Zenthrill.Providers/"]
COPY ["src/backend/Zenthrill.Application/Zenthrill.Application.csproj", "src/backend/Zenthrill.Application/"]
COPY ["src/backend-libs/Zenthrill.Specifications/Zenthrill.Specifications.csproj", "src/backend-libs/Zenthrill.Specifications/"]
COPY ["src/backend/Zenthrill.Domain/Zenthrill.Domain.csproj", "src/backend/Zenthrill.Domain/"]
RUN dotnet restore "src/backend/Zenthrill.Story.Consumer/Zenthrill.Story.Consumer.csproj"
COPY . .
WORKDIR "/src/src/backend/Zenthrill.Story.Consumer"
RUN dotnet build "Zenthrill.Story.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zenthrill.Story.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zenthrill.Story.Consumer.dll"]
