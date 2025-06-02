# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiar SOLUÇÃO e projetos individuais (melhora cache)
COPY ["GymPlusAPI.sln", "."]
COPY ["src/GymPlusAPI.API/*.csproj", "src/GymPlusAPI.API/"]
COPY ["src/GymPlusAPI.Application/*.csproj", "src/GymPlusAPI.Application/"]
COPY ["src/GymPlusAPI.Domain/*.csproj", "src/GymPlusAPI.Domain/"]
COPY ["src/GymPlusAPI.Infrastructure/*.csproj", "src/GymPlusAPI.Infrastructure/"]

# 2. Restaurar dependências
RUN dotnet restore "GymPlusAPI.sln"

# 3. Copiar todo o código-fonte
COPY . .

# 4. Publicar a API
RUN dotnet publish "src/GymPlusAPI.API/GymPlusAPI.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Estágio final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GymPlusAPI.API.dll"]