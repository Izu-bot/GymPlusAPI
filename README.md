# GymPlusAPI

API REST para gerenciamento de usuários, planilhas de treino e exercícios, desenvolvida em .NET 9, com autenticação JWT e persistência em PostgreSQL.

## Funcionalidades

- Cadastro, autenticação e gerenciamento de usuários
- Criação, edição, listagem e exclusão de planilhas de treino
- Criação, edição, listagem e exclusão de exercícios vinculados a planilhas
- Autenticação via JWT
- Validação de dados com FluentValidation
- Documentação automática com Scalar/OpenAPI

## Estrutura do Projeto

```
GymPlusAPI.sln
src/
  GymPlusAPI.API/           # Projeto principal da API
  GymPlusAPI.Application/   # Regras de negócio e validações
  GymPlusAPI.Domain/        # Entidades e interfaces de domínio
  GymPlusAPI.Infrastructure/# Persistência e segurança
config.env                  # Variáveis de ambiente (conexão com banco)
Dockerfile                  # Build e execução via Docker
```

## Variáveis de Ambiente

Crie um arquivo `config.env` na raiz do projeto com as seguintes variáveis:

```
DB_HOST=localhost
DB_PORT=5432
DB_NAME=gymplus
DB_USER=postgres
DB_PASSWORD=postgres
```

## Como rodar com Docker

1. **Build da imagem:**
   ```sh
   docker build -t gymplus-api .
   ```

2. **Executar o container:**
   ```sh
   docker run -d -p 80:80 --env-file=config.env gymplus-api
   ```

3. **Acesse a documentação:**
   - [http://localhost/scalar/v1](http://localhost/scalar/v1)

## Endpoints principais

- `POST /api/auth/login` — Login de usuário (retorna JWT)
- `POST /api/user` — Cadastro de usuário
- `GET /api/user` — Dados do usuário autenticado
- `POST /api/spreadsheet` — Criar planilha de treino
- `GET /api/spreadsheet` — Listar planilhas do usuário
- `POST /api/workout` — Criar exercício
- `GET /api/workout` — Listar exercícios do usuário

> **Obs:** Todos os endpoints (exceto login e cadastro) requerem autenticação via Bearer Token.

## Tecnologias

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core + PostgreSQL
- JWT Authentication
- FluentValidation
- Scalar (OpenAPI)
- Docker

---
