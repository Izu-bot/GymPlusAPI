# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de solução e projetos para aproveitar o cache do Docker
COPY ["GymPlusAPI.sln", "."]
COPY ["src/GymPlusAPI.API/*.csproj", "src/GymPlusAPI.API/"]
COPY ["src/GymPlusAPI.Application/*.csproj", "src/GymPlusAPI.Application/"]
COPY ["src/GymPlusAPI.Domain/*.csproj", "src/GymPlusAPI.Domain/"]
COPY ["src/GymPlusAPI.Infrastructure/*.csproj", "src/GymPlusAPI.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "GymPlusAPI.sln"

# Copiar todo o código-fonte
COPY . .

# Publicar a API
RUN dotnet publish "src/GymPlusAPI.API/GymPlusAPI.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Estágio final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Expor apenas a porta HTTP
EXPOSE 80

# Copiar arquivos publicados do estágio de build
COPY --from=build /app/publish .

# Definir variáveis de ambiente para rodar só em HTTP
ENV ASPNETCORE_URLS="http://+:80"
ENV ASPNETCORE_ENVIRONMENT=Development

# Definir o entrypoint
ENTRYPOINT ["dotnet", "GymPlusAPI.API.dll"]