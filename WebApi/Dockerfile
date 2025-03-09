FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Убираем fallback и очищаем кеши
ENV NUGET_FALLBACK_PACKAGES=""
RUN dotnet nuget locals all --clear

# Копируем .csproj и выполняем restore
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "WebApi/WebApi.csproj" --force --no-cache

# Копируем весь код
COPY . .
WORKDIR "/src/WebApi"

# Собираем приложение
RUN dotnet build "WebApi.csproj" -c Release -o /app/build
