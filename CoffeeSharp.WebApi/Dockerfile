# ���� ������
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# ����������� ����� ������� � ���� ������ ��������
COPY CoffeeSharp.sln ./
COPY CoffeeSharp.Application/*.csproj ./CoffeeSharp.Application/
COPY CoffeeSharp.Domain/*.csproj ./CoffeeSharp.Domain/
COPY CoffeeSharp.Infrastructure/*.csproj ./CoffeeSharp.Infrastructure/
COPY CoffeeSharp.WebApi/*.csproj ./CoffeeSharp.WebApi/

# �������������� ������������
RUN dotnet restore CoffeeSharp.sln

# ����������� ����� ��������� ����
COPY . .

# ������ � ���������� ������� WebApi
WORKDIR /app/CoffeeSharp.WebApi
RUN dotnet publish -c Release -o out

# ���� ���������� ������
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/CoffeeSharp.WebApi/out ./
ENTRYPOINT ["dotnet", "CoffeeSharp.WebApi.dll"]
