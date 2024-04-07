FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/Zenthrill.Auth.WebAPI/Zenthrill.Auth.WebAPI.csproj", "src/backend/Zenthrill.Auth.WebAPI/"]
COPY ["src/backend/Zenthrill.Auth.Model/Zenthrill.Auth.Model.csproj", "src/backend/Zenthrill.Auth.Model/"]
COPY ["src/backend-libs/Zenthrill.APIResponses/Zenthrill.APIResponses.csproj", "src/backend-libs/Zenthrill.APIResponses/"]
RUN dotnet restore "src/backend/Zenthrill.Auth.WebAPI/Zenthrill.Auth.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/backend/Zenthrill.Auth.WebAPI"
RUN dotnet build "Zenthrill.Auth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zenthrill.Auth.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zenthrill.Auth.WebAPI.dll"]
