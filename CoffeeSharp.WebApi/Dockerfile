# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копирование файла решения и всех файлов проектов
COPY CoffeeSharp.sln ./
COPY CoffeeSharp.Application/*.csproj ./CoffeeSharp.Application/
COPY CoffeeSharp.Domain/*.csproj ./CoffeeSharp.Domain/
COPY CoffeeSharp.Infrastructure/*.csproj ./CoffeeSharp.Infrastructure/
COPY CoffeeSharp.WebApi/*.csproj ./CoffeeSharp.WebApi/

# Восстановление зависимостей
RUN dotnet restore CoffeeSharp.sln

# Копирование всего исходного кода
COPY . .

# Сборка и публикация проекта WebApi
WORKDIR /app/CoffeeSharp.WebApi
RUN dotnet publish -c Release -o out

# Этап финального образа
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/CoffeeSharp.WebApi/out ./
ENTRYPOINT ["dotnet", "CoffeeSharp.WebApi.dll"]
