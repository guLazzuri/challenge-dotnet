# 🏍️ Challenge .NET - Sistema de Gestão de Veículos e Manutenção

> API RESTful completa para gerenciamento inteligente de usuários, veículos e históricos de manutenção, desenvolvida em ASP.NET Core 8 com arquitetura em camadas, segurança JWT, Health Checks e boas práticas.

## 📋 Visão Geral

O **Challenge .NET** é uma solução tecnológica robusta que oferece controle completo sobre gestão de frotas e manutenções de veículos. Desenvolvido com foco em escalabilidade, segurança e manutenibilidade, o sistema implementa padrões modernos de arquitetura, incluindo Repository Pattern, DTOs, HATEOAS, autenticação JWT, versionamento de API e separação clara de responsabilidades em camadas.

### 🎯 Problemas Solucionados

- **Gestão Descentralizada**: Centraliza todas as informações de veículos e manutenções em uma única plataforma
- **Falta de Rastreabilidade**: Fornece histórico completo e detalhado de todas as manutenções realizadas
- **Dados Não Estruturados**: Organiza informações de forma estruturada e acessível via API
- **Ausência de Automação**: Base sólida para integração com sistemas externos e automações
- **Segurança**: Implementa autenticação e autorização via JWT para proteger endpoints sensíveis

### 🚀 Objetivos da Solução

- **API RESTful Completa**: Endpoints bem definidos seguindo padrões REST e HATEOAS
- **Segurança Robusta**: Autenticação JWT Bearer Token para proteger recursos
- **Health Checks**: Monitoramento de saúde da aplicação e dependências
- **Versionamento**: Controle de versões da API para evolução sem breaking changes
- **Arquitetura em Camadas**: Separação clara entre apresentação, domínio e infraestrutura
- **Escalabilidade**: Estrutura preparada para crescimento e novas funcionalidades
- **Documentação Automática**: Swagger integrado para facilitar consumo da API
- **Testes Completos**: Testes unitários e de integração com xUnit

## 🔧 Funcionalidades Principais

### 🔐 Autenticação e Segurança
- **JWT Bearer Token**: Autenticação baseada em tokens JWT
- **Endpoints Protegidos**: Todos os recursos principais protegidos por autorização
- **Swagger com Autorização**: Interface Swagger integrada com autenticação JWT
- **Credenciais de Teste**:
  - Admin: `username: admin`, `password: admin123`
  - User: `username: user`, `password: user123`

### 💚 Health Checks
- **Endpoint Detalhado** (`/health`): Retorna status completo da aplicação e dependências
- **Endpoint Simplificado** (`/health/ready`): Para uso em load balancers
- **Endpoint Liveness** (`/health/live`): Verifica se a aplicação está viva
- **Monitoramento de Banco**: Verifica conectividade com Oracle Database
- **Resposta JSON Estruturada**: Informações detalhadas sobre cada componente

### 📌 Versionamento de API
- **Versionamento por URL**: Suporte a múltiplas versões (`/api/v1/...`)
- **Versionamento no Swagger**: Documentação separada por versão
- **Versão Padrão**: v1.0 assumida quando não especificada
- **Headers de Versão**: Informações de versão nos headers de resposta

### 👥 Gestão de Usuários
- Cadastro completo com validação de dados
- Suporte a diferentes tipos de usuário (Admin/Client)
- Operações CRUD completas
- Consultas com paginação
- Proteção por autenticação JWT

### 🚗 Controle de Veículos
- Registro detalhado de veículos
- Associação com proprietários
- Gerenciamento de modelos (E, SPORT, POP)
- Busca e filtros avançados
- Proteção por autenticação JWT

### 🔧 Histórico de Manutenções
- Registro completo de serviços realizados
- Vinculação com veículos específicos
- Controle de custos e quilometragem
- Timeline histórica de manutenções
- Sistema de cancelamento de registros
- Proteção por autenticação JWT

### 📊 Recursos Técnicos
- Paginação de resultados para performance
- HATEOAS para APIs auto-descritivas
- DTOs para transferência segura de dados
- Repository Pattern para abstração de dados
- Testes unitários com xUnit
- Testes de integração com WebApplicationFactory

## 🔗 Endpoints da API

### 🔐 Auth (Autenticação)
| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| POST | `/api/v1/auth/login` | Gera token JWT | Não |
| GET | `/api/v1/auth/validate` | Valida token JWT | Sim |

**Exemplo de Login:**
```json
POST /api/v1/auth/login
{
  "username": "admin",
  "password": "admin123"
}

Resposta:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "type": "Bearer",
  "expiresIn": 3600,
  "username": "admin"
}
```

### 💚 Health (Saúde da Aplicação)
| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/health` | Status detalhado da aplicação | Não |
| GET | `/health/ready` | Verifica se está pronta | Não |
| GET | `/health/live` | Verifica se está viva | Não |
| GET | `/api/v1/health` | Status via controller | Não |
| GET | `/api/v1/health/ready` | Ready via controller | Não |
| GET | `/api/v1/health/live` | Live via controller | Não |

### 👤 Users (Usuários)
| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/v1/user` | Lista todos os usuários (paginado) | Sim |
| GET | `/api/v1/user/{id}` | Obtém um usuário específico | Sim |
| POST | `/api/v1/user` | Cria um novo usuário | Sim |
| PUT | `/api/v1/user/{id}` | Atualiza um usuário existente | Sim |
| DELETE | `/api/v1/user/{id}` | Remove um usuário | Sim |

### 🚗 Vehicles (Veículos)
| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/v1/vehicles` | Lista todos os veículos (paginado) | Sim |
| GET | `/api/v1/vehicles/{id}` | Obtém um veículo específico | Sim |
| POST | `/api/v1/vehicles` | Cadastra um novo veículo | Sim |
| PUT | `/api/v1/vehicles/{id}` | Atualiza um veículo existente | Sim |
| DELETE | `/api/v1/vehicles/{id}` | Remove um veículo | Sim |

### 🛠️ MaintenanceHistories (Histórico de Manutenção)
| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/v1/maintenancehistories` | Lista todos os históricos (paginado) | Sim |
| GET | `/api/v1/maintenancehistories/{id}` | Obtém um histórico específico | Sim |
| POST | `/api/v1/maintenancehistories` | Registra uma nova manutenção | Sim |
| PUT | `/api/v1/maintenancehistories/{id}` | Atualiza um histórico | Sim |
| DELETE | `/api/v1/maintenancehistories/{id}` | Cancela um histórico de manutenção | Sim |

## 🏗️ Arquitetura e Tecnologias

### Stack Tecnológica
- **ASP.NET Core 8**: Framework principal para desenvolvimento web
- **Entity Framework Core**: ORM para acesso e mapeamento de dados
- **Oracle Database**: Banco de dados relacional robusto
- **JWT Bearer Authentication**: Autenticação baseada em tokens
- **Swagger/OpenAPI**: Documentação interativa automática
- **xUnit**: Framework de testes unitários e de integração
- **Moq**: Biblioteca para criação de mocks em testes
- **WebApplicationFactory**: Testes de integração end-to-end
- **Bootstrap 5**: Framework CSS para interface responsiva
- **jQuery**: Biblioteca JavaScript para interatividade

### Estrutura do Projeto

```
challenge-dotnet/
│
├── 📁 Controllers/                       # Camada de Apresentação
│   ├── AuthController.cs                 # Endpoints de autenticação JWT
│   ├── HealthController.cs               # Endpoints de health checks
│   ├── MaintenanceHistoriesController.cs # Endpoints de histórico
│   ├── UserController.cs                 # Endpoints de usuários
│   └── VehiclesController.cs             # Endpoints de veículos
│
├── 📁 Domain/                            # Camada de Domínio
│   ├── DTOs/                             # Data Transfer Objects
│   │   ├── LinkDto.cs                    # Links HATEOAS
│   │   ├── MaintenanceHistoryDto.cs      # DTO de manutenção
│   │   ├── PagedResult.cs                # Resultado paginado
│   │   ├── UserDto.cs                    # DTO de usuário
│   │   └── VehicleDto.cs                 # DTO de veículo
│   │
│   ├── Entity/                           # Entidades do Domínio
│   │   ├── MaintenanceHistory.cs         # Entidade de histórico
│   │   ├── User.cs                       # Entidade de usuário
│   │   ├── UserType.cs                   # Enum de tipo de usuário
│   │   ├── Vehicle.cs                    # Entidade de veículo
│   │   └── VehicleModel.cs               # Modelo de veículo
│   │
│   └── Interfaces/                       # Contratos do Domínio
│       └── ICancel.cs                    # Interface de cancelamento
│
├── 📁 Infrastructure/                    # Camada de Infraestrutura
│   ├── Context/                          # Contexto do Banco
│   │   └── ChallengeContext.cs           # DbContext principal
│   │
│   ├── Mappings/                         # Configurações EF Core
│   │   ├── MaintenanceHistoryMapping.cs  # Mapeamento de histórico
│   │   ├── UserMapping.cs                # Mapeamento de usuário
│   │   └── VehicleMapping.cs             # Mapeamento de veículo
│   │
│   ├── Persistence/Repositories/         # Acesso a Dados
│   │   ├── IRepository.cs                # Interface genérica
│   │   └── Repository.cs                 # Implementação genérica
│   │
│   └── Services/                         # Serviços de Infraestrutura
│       └── HateoasService.cs             # Serviço de links HATEOAS
│
├── 📁 Challenge.Tests/                   # Projeto de Testes
│   ├── AuthControllerTests.cs            # Testes de autenticação
│   ├── HealthControllerTests.cs          # Testes de health checks
│   ├── IntegrationTests.cs               # Testes de integração
│   ├── MaintenanceHistoriesControllerTests.cs
│   ├── UserControllerTests.cs
│   ├── VehiclesControllerTests.cs
│   └── Challenge.Tests.csproj
│
├── 📁 Migrations/                        # Migrações do Banco de Dados
├── 📁 wwwroot/                           # Arquivos Estáticos
│   ├── css/                              # Estilos CSS
│   ├── js/                               # Scripts JavaScript
│   └── lib/                              # Bibliotecas (Bootstrap, jQuery)
│
├── appsettings.json                      # Configurações Gerais
├── appsettings.Development.json          # Configurações de Desenvolvimento
├── Program.cs                            # Ponto de Entrada da Aplicação
└── README.md                             # Este arquivo
```

### Padrões Implementados
- **Repository Pattern**: Abstração de acesso a dados
- **DTO Pattern**: Transferência segura de dados entre camadas
- **Dependency Injection**: Inversão de controle nativa do .NET
- **RESTful API**: Arquitetura REST com HATEOAS
- **Clean Architecture**: Separação clara de responsabilidades
- **Code First Migrations**: Controle de versão do banco de dados
- **JWT Authentication**: Autenticação stateless baseada em tokens
- **Health Check Pattern**: Monitoramento de saúde da aplicação
- **API Versioning**: Versionamento para evolução controlada

## ⚙️ Instalação e Configuração

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [Oracle Database](https://www.oracle.com/database/) ou acesso a instância Oracle
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) para controle de versão

### Passos de Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/guLazzuri/challenge-dotnet.git
   cd challenge-dotnet
   ```

2. **Configure a conexão com o banco de dados**
   
   Edite o arquivo `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "Oracle": "Data Source=seu-servidor:1521/orcl;User ID=seu-usuario;Password=sua-senha;"
     },
     "Jwt": {
       "Key": "SuaChaveSecretaAqui_MinimoDeCaracteres",
       "Issuer": "ChallengeAPI",
       "Audience": "ChallengeAPIUsers",
       "ExpirationHours": 1
     }
   }
   ```

3. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

4. **Execute as migrações do banco de dados**
   ```bash
   dotnet ef database update
   ```

5. **Execute a aplicação**
   ```bash
   dotnet run
   ```

6. **Acesse a documentação Swagger**
   ```
   https://localhost:7000/swagger
   ```

## 🧪 Executando os Testes

### Testes Unitários

Execute todos os testes unitários:
```bash
git dotnet test
```

Execute testes com cobertura de código:
```bash
dotnet test /p:CollectCoverage=true
```

Execute testes de um arquivo específico:
```bash
dotnet test --filter "FullyQualifiedName~AuthControllerTests"
```

### Testes de Integração

Os testes de integração utilizam `WebApplicationFactory` para criar uma instância completa da aplicação em memória:

```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

### Estrutura dos Testes

O projeto inclui:

- ✅ **Testes Unitários**:
  - `AuthControllerTests.cs`: Testa autenticação JWT
  - `HealthControllerTests.cs`: Testa health checks
  - `VehiclesControllerTests.cs`: Testa operações CRUD de veículos
  - `UserControllerTests.cs`: Testa operações CRUD de usuários
  - `MaintenanceHistoriesControllerTests.cs`: Testa operações de manutenção

- ✅ **Testes de Integração**:
  - `IntegrationTests.cs`: Testa fluxos completos end-to-end
    - Health checks
    - Autenticação JWT
    - Endpoints protegidos
    - Swagger acessível

### Executar Testes com Detalhes

Para ver logs detalhados durante a execução dos testes:
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Gerar Relatório de Testes

```bash
dotnet test --logger "trx;LogFileName=test-results.trx"
```

## 🔐 Usando a API com Autenticação

### 1. Obter Token JWT

**Requisição:**
```bash
curl -X POST https://localhost:7000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123"
  }'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "type": "Bearer",
  "expiresIn": 3600,
  "username": "admin"
}
```

### 2. Usar Token em Requisições

**Com cURL:**
```bash
curl -X GET https://localhost:7000/api/v1/vehicles \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

**Com Swagger:**
1. Clique no botão **"Authorize"** no topo da página
2. Insira: `Bearer SEU_TOKEN_AQUI`
3. Clique em **"Authorize"**
4. Agora você pode testar todos os endpoints protegidos

**Com Postman:**
1. Vá para a aba **"Authorization"**
2. Selecione **"Bearer Token"**
3. Cole o token no campo **"Token"**

### 3. Validar Token

```bash
curl -X GET https://localhost:7000/api/v1/auth/validate \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

## 💚 Monitoramento com Health Checks

### Endpoints Disponíveis

**Health Check Detalhado:**
```bash
curl https://localhost:7000/health
```

**Resposta:**
```json
{
  "status": "Healthy",
  "timestamp": "2025-11-06T19:30:00Z",
  "duration": "00:00:00.1234567",
  "checks": [
    {
      "name": "oracle",
      "status": "Healthy",
      "description": "Oracle database connection",
      "duration": "00:00:00.0987654",
      "exception": null,
      "data": {}
    }
  ],
  "version": "1.0.0",
  "environment": "Development"
}
```

**Health Check Simplificado (para Load Balancers):**
```bash
curl https://localhost:7000/health/ready
```

**Liveness Probe (para Kubernetes):**
```bash
curl https://localhost:7000/health/live
```

### Integração com Monitoramento

Os health checks podem ser integrados com:
- **Kubernetes**: Liveness e Readiness Probes
- **Docker**: HEALTHCHECK instruction
- **Load Balancers**: Health check endpoints
- **Monitoring Tools**: Prometheus, Grafana, etc.

## 📚 Documentação da API

### Swagger UI

Acesse a documentação interativa em:
```
https://localhost:7000/swagger
```

A interface Swagger oferece:
- 📖 Documentação completa de todos os endpoints
- 🧪 Teste interativo de requisições
- 🔐 Suporte a autenticação JWT
- 📋 Schemas de dados detalhados
- 💡 Exemplos de requisições e respostas

### OpenAPI Specification

O arquivo JSON da especificação OpenAPI está disponível em:
```
https://localhost:7000/swagger/v1/swagger.json
```

## 🚀 Deploy

### Publicação

```bash
dotnet publish -c Release -o ./publish
```

### Docker

Crie um `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["challenge.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "challenge.dll"]
```

Build e execute:
```bash
docker build -t challenge-dotnet .
docker run -p 8080:80 challenge-dotnet
```

## 📊 Métricas e Qualidade

### Cobertura de Testes
- ✅ Testes unitários para todos os controllers
- ✅ Testes de integração end-to-end
- ✅ Testes de autenticação e autorização
- ✅ Testes de health checks
- ✅ Mocks para dependências externas

### Boas Práticas Implementadas
- ✅ Clean Architecture
- ✅ SOLID Principles
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ RESTful API Design
- ✅ HATEOAS
- ✅ JWT Authentication
- ✅ API Versioning
- ✅ Health Checks
- ✅ Comprehensive Testing
- ✅ Swagger Documentation

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👨‍💻 Autor

**Gustavo Lazzuri**
- Email: gulazzuri@gmail.com
- GitHub: [@guLazzuri](https://github.com/guLazzuri)

## 🎓 Projeto Acadêmico

Este projeto foi desenvolvido como parte do curso **Advanced Business Development with .NET** da FIAP.

### Funcionalidades Implementadas (4º Sprint)

- ✅ **10 pts** - Health Checks implementado com endpoint detalhado
- ✅ **10 pts** - Versionamento da API implementado (v1)
- ✅ **25 pts** - Segurança JWT implementada com autenticação completa
- ✅ **30 pts** - Testes unitários com xUnit para todos os controllers
- ✅ **30 pts** - Testes de integração com WebApplicationFactory
- ✅ **Bonus** - Documentação Swagger atualizada com autenticação
- ✅ **Bonus** - README completo com instruções de teste

---

⭐ Se este projeto foi útil para você, considere dar uma estrela no GitHub!
