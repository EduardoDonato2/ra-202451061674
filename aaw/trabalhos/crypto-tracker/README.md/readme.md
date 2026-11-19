# CryptoTracker API

API RESTful desenvolvida em **.NET 10 (ASP.NET Core)** com **MongoDB** e autenticação **JWT**, voltada para o gerenciamento de carteiras de criptoativos e controle automatizado de transações de compra e venda.

## Arquitetura e Tecnologias

- **Linguagem / Framework:** C# / .NET 10 Web API
- **Banco de Dados:** MongoDB (via Docker Compose)
- **Segurança:** Autenticação e Autorização via JWT (HMAC-SHA256)
- **Documentação Interativa:** Swagger / OpenAPI
- **Testes Automatizados:** xUnit + Moq
- **Padrão Arquitetural:** Camadas desacopladas (`Controller` → `Service` → `Repository`) com Injeção de Dependência.

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (com WSL2 ativo)

---

## Como Executar o Projeto

### 1. Subir o Banco de Dados (MongoDB)
Na raiz do projeto (`crypto-tracker`), inicie o container do MongoDB:
```bash
docker compose up -d