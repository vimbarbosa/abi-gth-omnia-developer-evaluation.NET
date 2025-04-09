# 💻 Ambev Developer Evaluation (.NET)

Este é o backend da aplicação de avaliação técnica, implementado em .NET 8 com arquitetura em camadas, suporte a mensageria via RabbitMQ, persistência com PostgreSQL e migrations gerenciadas separadamente via projeto `ORM`.

---

## 📦 Tecnologias Utilizadas

- ASP.NET Core 8 (Web API)
- Entity Framework Core (com projeto separado para Migrations)
- PostgreSQL
- RabbitMQ
- Redis (opcional, usado como cache)
- Docker / Docker Compose

---

## 🚀 Como rodar o projeto localmente

### 1. 📁 Clone o repositório

bash
git clone https://github.com/vimbarbosa/abi-gth-omnia-developer-evaluation.NET.git
cd abi-gth-omnia-developer-evaluation.NET

### 2. ⚙️ Pré-requisitos
Certifique-se de ter instalado:

- .NET SDK 8+
- Docke
- EF CLI:
    dotnet tool install --global dotnet-ef

### 3. 🐳 Inicie com o script start.sh

chmod +x start.sh
./start.sh

### 4. 🌐 Acesse a API
http://localhost:5119/swagger

### 🧪 Testes
 - Para rodar os testes unitários:
dotnet test

### 🗃️ Migrations
⚠️ As migrations são gerenciadas via o projeto Ambev.DeveloperEvaluation.ORM
 - Criar uma nova migration
dotnet ef migrations add NomeDaMigration \
  --project src/Ambev.DeveloperEvaluation.ORM \
  --output-dir Migrations

- Aplicar migrations manualmente (caso necessário)
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM

### 📁 Estrutura de Pastas
/src
│
├── Ambev.DeveloperEvaluation.WebApi      # Projeto da API (Endpoints, Controllers, Middlewares)
├── Ambev.DeveloperEvaluation.ORM         # Projeto do EF Core (DbContext + Migrations)
├── Ambev.DeveloperEvaluation.Domain      # Entidades, Eventos, Interfaces
├── Ambev.DeveloperEvaluation.Application # Commands, Handlers, Validations
└── Ambev.DeveloperEvaluation.Unit        # Testes Unitários (xUnit)

✅ Ambiente
Variáveis de ambiente estão definidas nos arquivos appsettings.json e/ou docker-compose.yml. Por padrão:
PostgreSQL: localhost:5432
RabbitMQ: localhost:5672, com UI em localhost:15672 (user: developer, password: ev@luAt10n)
Redis: localhost:6379

### 🧼 Reset do Banco de Dados
- Se quiser resetar o banco e gerar uma nova migration:
dotnet ef migrations remove --project src/Ambev.DeveloperEvaluation.ORM
dotnet ef migrations add Initial --project src/Ambev.DeveloperEvaluation.ORM
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM