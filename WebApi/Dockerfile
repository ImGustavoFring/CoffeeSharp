# Используем последний образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файлы проектов и восстанавливаем зависимости
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]
RUN dotnet restore "WebApi/WebApi.csproj"

# Копируем все файлы решения
COPY . .

# Собираем проект WebApi
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

# Публикуем проект WebApi
FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

# Используем образ ASP.NET Core для запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]