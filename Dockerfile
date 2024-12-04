# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HallOfFame.API/HallOfFame.API.csproj", "src/HallOfFame.API/"]
COPY ["src/HallOfFame.Contracts/HallOfFame.Contracts.csproj", "src/HallOfFame.Contracts/"]
COPY ["src/HallOfFame.Core/HallOfFame.Core.csproj", "src/HallOfFame.Core/"]
COPY ["src/HallOfFame.DataAccess/HallOfFame.DataAccess.csproj", "src/HallOfFame.DataAccess/"]
COPY ["src/HallOfFame.UseCases/HallOfFame.UseCases.csproj", "src/HallOfFame.UseCases/"]
RUN dotnet restore "./src/HallOfFame.API/HallOfFame.API.csproj"
COPY . .
WORKDIR "/src/src/HallOfFame.API"
RUN dotnet build "./HallOfFame.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HallOfFame.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HallOfFame.API.dll"]