FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/Zenthrill.WebAPI/Zenthrill.WebAPI.csproj", "src/backend/Zenthrill.WebAPI/"]
COPY ["src/backend-libs/Zenthrill.APIResponses/Zenthrill.APIResponses.csproj", "src/backend-libs/Zenthrill.APIResponses/"]
COPY ["src/backend-libs/Zenthrill.Outbox.Publisher/Zenthrill.Outbox.Publisher.csproj", "src/backend-libs/Zenthrill.Outbox.Publisher/"]
COPY ["src/backend-libs/Zenthrill.Outbox.Core/Zenthrill.Outbox.Core.csproj", "src/backend-libs/Zenthrill.Outbox.Core/"]
COPY ["src/backend-libs/Zenthrill.Outbox.EntityFrameworkCore/Zenthrill.Outbox.EntityFrameworkCore.csproj", "src/backend-libs/Zenthrill.Outbox.EntityFrameworkCore/"]
COPY ["src/backend-libs/Zenthrill.Providers/Zenthrill.Providers.csproj", "src/backend-libs/Zenthrill.Providers/"]
COPY ["src/backend-libs/Zenthrill.Settings.DependencyInjection/Zenthrill.Settings.DependencyInjection.csproj", "src/backend-libs/Zenthrill.Settings.DependencyInjection/"]
COPY ["src/backend-libs/Zenthrill.Settings/Zenthrill.Settings.csproj", "src/backend-libs/Zenthrill.Settings/"]
COPY ["src/backend/Zenthrill.Infrastructure/Zenthrill.Infrastructure.csproj", "src/backend/Zenthrill.Infrastructure/"]
COPY ["src/backend-libs/Zenthrill.Providers.DependencyInjection/Zenthrill.Providers.DependencyInjection.csproj", "src/backend-libs/Zenthrill.Providers.DependencyInjection/"]
COPY ["src/backend/Zenthrill.Application/Zenthrill.Application.csproj", "src/backend/Zenthrill.Application/"]
COPY ["src/backend-libs/Zenthrill.Specifications/Zenthrill.Specifications.csproj", "src/backend-libs/Zenthrill.Specifications/"]
COPY ["src/backend/Zenthrill.Domain/Zenthrill.Domain.csproj", "src/backend/Zenthrill.Domain/"]
RUN dotnet restore "src/backend/Zenthrill.WebAPI/Zenthrill.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/backend/Zenthrill.WebAPI"
RUN dotnet build "Zenthrill.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zenthrill.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zenthrill.WebAPI.dll"]
