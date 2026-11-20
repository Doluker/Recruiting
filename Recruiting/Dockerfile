# СТАДИЯ 1: Сборка и публикация
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем только файлы проектов для кэширования слоёв nuget-пакетов
COPY ["Recruiting/Recruiting.csproj", "Recruiting/"]
COPY ["Recruiting.Client/Recruiting.Client.csproj", "Recruiting.Client/"]
COPY ["Recruiting.Shared/Recruiting.Shared.csproj", "Recruiting.Shared/"]

RUN dotnet restore "Recruiting/Recruiting.csproj"

# Копируем весь исходный код
COPY . .

# Собираем и публикуем (бэкенд автоматически опубликует и скомпилирует Blazor WebAssembly клиент)
WORKDIR "/src/Recruiting"
RUN dotnet publish "Recruiting.csproj" -c Release -o /app/publish /p:UseAppHost=false

# СТАДИЯ 2: Финальный легковесный рантайм
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Recruiting.dll"]