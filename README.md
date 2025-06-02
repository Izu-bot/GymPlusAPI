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
