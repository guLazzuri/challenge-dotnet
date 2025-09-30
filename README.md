# Challenge .NET - API de Gerenciamento de Veículos e Manutenção

Uma API RESTful completa desenvolvida em .NET 8 para gerenciar usuários, veículos e seus históricos de manutenção, implementando boas práticas de arquitetura em camadas e padrões HATEOAS.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Executando o Projeto](#executando-o-projeto)
- [Endpoints da API](#endpoints-da-api)
- [Exemplos de Uso](#exemplos-de-uso)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

## 🎯 Sobre o Projeto

O **Challenge .NET** é uma API robusta que oferece uma solução completa para gestão de frotas e manutenções de veículos. O sistema permite cadastrar usuários, registrar veículos e acompanhar todo o histórico de manutenções realizadas, facilitando o controle e organização de oficinas mecânicas, empresas de logística ou frotas corporativas.

### Principais Diferenciais

- Arquitetura em camadas bem definida (Controllers, Domain, Infrastructure)
- Implementação de HATEOAS para APIs auto-descritivas
- Padrão Repository para abstração de acesso a dados
- Entity Framework Core com Code First Migrations
- DTOs para transferência segura de dados
- Suporte a paginação de resultados

## ✨ Funcionalidades

### Gestão de Usuários
- Cadastro, consulta, atualização e remoção de usuários
- Suporte a diferentes tipos de usuário
- Validação de dados de entrada

### Gestão de Veículos
- Registro completo de veículos
- Associação de veículos a usuários
- Gerenciamento de modelos de veículos
- Consultas com paginação

### Histórico de Manutenções
- Registro detalhado de manutenções realizadas
- Vinculação de manutenções a veículos específicos
- Consulta histórica com filtros
- Possibilidade de cancelamento de registros
- Timeline completa de manutenções por veículo

## 🚀 Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core** - Framework web
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados relacional
- **C#** - Linguagem de programação
- **HATEOAS** - Padrão de API REST
- **Bootstrap 5** - Framework CSS (frontend)
- **jQuery** - Biblioteca JavaScript
- **Swagger/OpenAPI** - Documentação de API

## 📁 Estrutura do Projeto

```
challenge-dotnet/
│
├── Controllers/                          # Camada de apresentação
│   ├── MaintenanceHistoriesController.cs # Endpoints de histórico
│   ├── UserController.cs                 # Endpoints de usuários
│   └── VehiclesController.cs             # Endpoints de veículos
│
├── Domain/                               # Camada de domínio
│   ├── DTOs/                             # Data Transfer Objects
│   │   ├── LinkDto.cs                    # Links HATEOAS
│   │   ├── MaintenanceHistoryDto.cs      # DTO de manutenção
│   │   ├── PagedResult.cs                # Resultado paginado
│   │   ├── UserDto.cs                    # DTO de usuário
│   │   └── VehicleDto.cs                 # DTO de veículo
│   │
│   ├── Entity/                           # Entidades do domínio
│   │   ├── MaintenanceHistory.cs         # Entidade de histórico
│   │   ├── User.cs                       # Entidade de usuário
│   │   ├── UserType.cs                   # Enum de tipo de usuário
│   │   ├── Vehicle.cs                    # Entidade de veículo
│   │   └── VehicleModel.cs               # Modelo de veículo
│   │
│   └── Interfaces/                       # Contratos do domínio
│       └── ICancel.cs                    # Interface de cancelamento
│
├── Infrastructure/                       # Camada de infraestrutura
│   ├── Context/                          # Contexto do banco
│   │   └── ChallengeContext.cs           # DbContext principal
│   │
│   ├── Mappings/                         # Configurações EF Core
│   │   ├── MaintenanceHistoryMapping.cs  # Mapeamento de histórico
│   │   ├── UserMapping.cs                # Mapeamento de usuário
│   │   └── VehicleMapping.cs             # Mapeamento de veículo
│   │
│   ├── Persistence/Repositories/         # Acesso a dados
│   │   ├── IRepository.cs                # Interface genérica
│   │   └── Repository.cs                 # Implementação genérica
│   │
│   └── Services/                         # Serviços de infraestrutura
│       └── HateoasService.cs             # Serviço de links HATEOAS
│
├── Migrations/                           # Migrações do banco de dados
│
├── wwwroot/                              # Arquivos estáticos
│   ├── css/                              # Estilos CSS
│   ├── js/                               # Scripts JavaScript
│   └── lib/                              # Bibliotecas (Bootstrap, jQuery)
│
├── appsettings.json                      # Configurações gerais
├── appsettings.Development.json          # Configurações de desenvolvimento
├── launchSettings.json                   # Configurações de execução
└── Program.cs                            # Ponto de entrada da aplicação
```

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express ou Developer Edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) para controle de versão

## ⚙️ Instalação e Configuração

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/challenge-dotnet.git
cd challenge-dotnet
```

### 2. Configure a String de Conexão

Edite o arquivo `appsettings.json` e configure a conexão com seu banco de dados:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ChallengeDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Para ambientes de desenvolvimento, você também pode editar `appsettings.Development.json`.

### 3. Restaure as Dependências

```bash
dotnet restore
```

### 4. Aplique as Migrações do Banco de Dados

```bash
dotnet ef database update
```

Se você não tiver a ferramenta EF Core instalada globalmente:

```bash
dotnet tool install --global dotnet-ef
```

## 🏃 Executando o Projeto

### Modo Desenvolvimento

```bash
dotnet run
```

Ou, se estiver usando Visual Studio, pressione `F5` para executar com debug.

A API estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Acessar a Documentação Swagger

Após iniciar a aplicação, acesse:

```
https://localhost:5001/swagger
```

## 🔌 Endpoints da API

### Usuários

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/users` | Lista todos os usuários (paginado) |
| GET | `/api/users/{id}` | Obtém um usuário específico |
| POST | `/api/users` | Cria um novo usuário |
| PUT | `/api/users/{id}` | Atualiza um usuário existente |
| DELETE | `/api/users/{id}` | Remove um usuário |

### Veículos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/vehicles` | Lista todos os veículos (paginado) |
| GET | `/api/vehicles/{id}` | Obtém um veículo específico |
| POST | `/api/vehicles` | Cadastra um novo veículo |
| PUT | `/api/vehicles/{id}` | Atualiza um veículo existente |
| DELETE | `/api/vehicles/{id}` | Remove um veículo |

### Histórico de Manutenções

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/maintenancehistories` | Lista todos os históricos (paginado) |
| GET | `/api/maintenancehistories/{id}` | Obtém um histórico específico |
| GET | `/api/maintenancehistories/vehicle/{vehicleId}` | Lista históricos de um veículo |
| POST | `/api/maintenancehistories` | Registra uma nova manutenção |
| PUT | `/api/maintenancehistories/{id}` | Atualiza um histórico |
| DELETE | `/api/maintenancehistories/{id}` | Cancela um histórico de manutenção |

## 💡 Exemplos de Uso

### Criar um Novo Usuário

```bash
curl -X POST https://localhost:5001/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "userType": 1
  }'
```

### Cadastrar um Veículo

```bash
curl -X POST https://localhost:5001/api/vehicles \
  -H "Content-Type: application/json" \
  -d '{
    "licensePlate": "ABC-1234",
    "model": "Honda Civic",
    "year": 2023,
    "userId": 1
  }'
```

### Registrar uma Manutenção

```bash
curl -X POST https://localhost:5001/api/maintenancehistories \
  -H "Content-Type: application/json" \
  -d '{
    "vehicleId": 1,
    "description": "Troca de óleo e filtros",
    "date": "2024-03-15T10:00:00",
    "cost": 350.00,
    "mileage": 15000
  }'
```

### Listar Veículos com Paginação

```bash
curl -X GET "https://localhost:5001/api/vehicles?page=1&pageSize=10"
```

### Consultar Histórico de um Veículo

```bash
curl -X GET https://localhost:5001/api/maintenancehistories/vehicle/1
```

## 🤝 Contribuindo

Contribuições são sempre bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

### Diretrizes de Contribuição

- Mantenha o código limpo e bem documentado
- Siga os padrões de código do projeto
- Adicione testes para novas funcionalidades
- Atualize a documentação quando necessário
- Descreva claramente as mudanças no Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👨‍💻 Equipe de Desenvolvimento

| Desenvolvedor | RM |
|--------------|-----|
| Gustavo Lazzuri | 556772 |
| Eduardo Nagado | 558158 |
| Felipe Silva | 555307 |

## 📞 Contato

Para dúvidas ou sugestões, entre em contato através do email: contato@exemplo.com

---

Desenvolvido com ❤️ usando .NET 8
