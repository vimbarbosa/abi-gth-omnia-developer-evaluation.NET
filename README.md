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
  - bash
    dotnet tool install --global dotnet-ef

### 3. 🐳 Inicie com o script start.sh

- abra o bash na raiz do projeto e execute o comando
./start.sh

### 4. 🌐 Acesse a API
http://localhost:5119/swagger


### 5 🐰 Acessando o RabbitMQ
A aplicação sobe um container com RabbitMQ que inclui o painel de gerenciamento web. Para acessá-lo:
URL: http://localhost:15672
Usuário: developer
Senha: ev@luAt10n