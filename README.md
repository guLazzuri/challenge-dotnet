# 🏍️ Challenge .NET - Sistema de Gestão de Veículos e Manutenção.

> API RESTful completa para gerenciamento inteligente de usuários, veículos e históricos de manutenção, desenvolvida em ASP.NET Core 8 com arquitetura em camadas e boas práticas.

## 📋 Visão Geral

O **Challenge .NET** é uma solução tecnológica robusta que oferece controle completo sobre gestão de frotas e manutenções de veículos. Desenvolvido com foco em escalabilidade e manutenibilidade, o sistema implementa padrões modernos de arquitetura, incluindo Repository Pattern, DTOs, HATEOAS e separação clara de responsabilidades em camadas.

### 🎯 Problemas Solucionados

- **Gestão Descentralizada**: Centraliza todas as informações de veículos e manutenções em uma única plataforma
- **Falta de Rastreabilidade**: Fornece histórico completo e detalhado de todas as manutenções realizadas
- **Dados Não Estruturados**: Organiza informações de forma estruturada e acessível via API
- **Ausência de Automação**: Base sólida para integração com sistemas externos e automações

### 🚀 Objetivos da Solução

- **API RESTful Completa**: Endpoints bem definidos seguindo padrões REST e HATEOAS
- **Arquitetura em Camadas**: Separação clara entre apresentação, domínio e infraestrutura
- **Escalabilidade**: Estrutura preparada para crescimento e novas funcionalidades
- **Documentação Automática**: Swagger integrado para facilitar consumo da API

## 🔧 Funcionalidades Principais

### 👥 Gestão de Usuários
- Cadastro completo com validação de dados
- Suporte a diferentes tipos de usuário
- Operações CRUD completas
- Consultas com paginação

### 🚗 Controle de Veículos
- Registro detalhado de veículos
- Associação com proprietários
- Gerenciamento de modelos
- Busca e filtros avançados

### 🔧 Histórico de Manutenções
- Registro completo de serviços realizados
- Vinculação com veículos específicos
- Controle de custos e quilometragem
- Timeline histórica de manutenções
- Sistema de cancelamento de registros

### 📊 Recursos Técnicos
- Paginação de resultados para performance
- HATEOAS para APIs auto-descritivas
- DTOs para transferência segura de dados
- Repository Pattern para abstração de dados

## 🔗 Endpoints da API

### 👤 Users (Usuários)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/users` | Lista todos os usuários (paginado) |
| GET | `/api/users/{id}` | Obtém um usuário específico |
| POST | `/api/users` | Cria um novo usuário |
| PUT | `/api/users/{id}` | Atualiza um usuário existente |
| DELETE | `/api/users/{id}` | Remove um usuário |

### 🚗 Vehicles (Veículos)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/vehicles` | Lista todos os veículos (paginado) |
| GET | `/api/vehicles/{id}` | Obtém um veículo específico |
| POST | `/api/vehicles` | Cadastra um novo veículo |
| PUT | `/api/vehicles/{id}` | Atualiza um veículo existente |
| DELETE | `/api/vehicles/{id}` | Remove um veículo |

### 🛠️ MaintenanceHistories (Histórico de Manutenção)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/maintenancehistories` | Lista todos os históricos (paginado) |
| GET | `/api/maintenancehistories/{id}` | Obtém um histórico específico |
| GET | `/api/maintenancehistories/vehicle/{vehicleId}` | Lista históricos de um veículo |
| POST | `/api/maintenancehistories` | Registra uma nova manutenção |
| PUT | `/api/maintenancehistories/{id}` | Atualiza um histórico |
| DELETE | `/api/maintenancehistories/{id}` | Cancela um histórico de manutenção |

## 🏗️ Arquitetura e Tecnologias

### Stack Tecnológica
- **ASP.NET Core 8**: Framework principal para desenvolvimento web
- **Entity Framework Core**: ORM para acesso e mapeamento de dados
- **SQL Server**: Banco de dados relacional robusto
- **Swagger/OpenAPI**: Documentação interativa automática
- **Bootstrap 5**: Framework CSS para interface responsiva
- **jQuery**: Biblioteca JavaScript para interatividade

### Estrutura do Projeto

```
challenge-dotnet/
│
├── 📁 Controllers/                       # Camada de Apresentação
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
├── 📁 Migrations/                        # Migrações do Banco de Dados
├── 📁 wwwroot/                           # Arquivos Estáticos
│   ├── css/                              # Estilos CSS
│   ├── js/                               # Scripts JavaScript
│   └── lib/                              # Bibliotecas (Bootstrap, jQuery)
│
├── appsettings.json                      # Configurações Gerais
├── appsettings.Development.json          # Configurações de Desenvolvimento
└── Program.cs                            # Ponto de Entrada da Aplicação
```

### Padrões Implementados
- **Repository Pattern**: Abstração de acesso a dados
- **DTO Pattern**: Transferência segura de dados entre camadas
- **Dependency Injection**: Inversão de controle nativa do .NET
- **RESTful API**: Arquitetura REST com HATEOAS
- **Clean Architecture**: Separação clara de responsabilidades
- **Code First Migrations**: Controle de versão do banco de dados

## 🏛️ Justificativa da Arquitetura

### Arquitetura em Camadas

O projeto foi estruturado seguindo os princípios da **Clean Architecture** com separação em três camadas principais:

#### 1️⃣ **Camada de Apresentação (Controllers)**
- **Responsabilidade**: Receber requisições HTTP, validar entrada e retornar respostas
- **Justificativa**: Isola a lógica de comunicação HTTP do restante da aplicação, facilitando mudanças na interface sem afetar a lógica de negócio
- **Benefício**: Permite trocar o framework web (ex: de ASP.NET para outra tecnologia) sem impactar outras camadas

#### 2️⃣ **Camada de Domínio (Domain)**
- **Responsabilidade**: Contém as entidades, DTOs, interfaces e regras de negócio
- **Justificativa**: Centraliza toda a lógica de negócio e modelos de dados, mantendo-os independentes de frameworks externos
- **Benefício**: Facilita testes unitários e garante que as regras de negócio sejam reutilizáveis

#### 3️⃣ **Camada de Infraestrutura (Infrastructure)**
- **Responsabilidade**: Implementa acesso a dados, serviços externos e configurações de persistência
- **Justificativa**: Separa detalhes técnicos (banco de dados, APIs externas) da lógica de negócio
- **Benefício**: Permite trocar o banco de dados (ex: SQL Server para Oracle) alterando apenas esta camada

### Padrões de Projeto Adotados

#### **Repository Pattern**
- **Por quê**: Abstrai a lógica de acesso a dados, tornando o código mais testável e desacoplado
- **Vantagem**: Facilita a criação de testes unitários mockando repositórios
- **Implementação**: Interface `IRepository<T>` genérica com implementação concreta

#### **DTO (Data Transfer Objects)**
- **Por quê**: Evita exposição direta das entidades de domínio nas APIs
- **Vantagem**: Controla exatamente quais dados são enviados/recebidos, melhorando segurança
- **Implementação**: DTOs específicos para cada operação (UserDto, VehicleDto, etc.)

#### **Dependency Injection**
- **Por quê**: Promove baixo acoplamento e facilita testes
- **Vantagem**: Permite substituir implementações facilmente (ex: trocar repositório real por mock em testes)
- **Implementação**: Injeção nativa do ASP.NET Core via `IServiceCollection`

#### **HATEOAS (Hypermedia as the Engine of Application State)**
- **Por quê**: Torna a API auto-descritiva, guiando o cliente através de links
- **Vantagem**: Cliente não precisa conhecer URLs fixas, apenas seguir links fornecidos
- **Implementação**: `HateoasService` adiciona links relevantes em cada resposta

### Decisões Técnicas

| Decisão | Justificativa |
|---------|---------------|
| **Entity Framework Core** | ORM maduro, bem documentado, com suporte robusto a migrações e LINQ |
| **SQL Server** | Banco robusto, escalável e com ótima integração com .NET |
| **Swagger/OpenAPI** | Documentação automática, facilita testes e integração com frontend |
| **Code First Migrations** | Controle de versão do schema do banco via código, facilitando deploys |

### Escalabilidade e Manutenibilidade

A arquitetura escolhida permite:
- ✅ **Adicionar novos endpoints** sem impactar código existente
- ✅ **Trocar tecnologias** (banco de dados, framework) com mínimo impacto
- ✅ **Escrever testes** facilmente devido ao baixo acoplamento
- ✅ **Trabalhar em equipe** com responsabilidades bem definidas por camada
- ✅ **Evoluir gradualmente** adicionando features sem refatorações massivas

## ⚙️ Instalação e Configuração

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express ou Developer Edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) para controle de versão

### Passos de Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/challenge-dotnet.git
   cd challenge-dotnet
   ```

2. **Configure a conexão com o banco de dados**
   
   Edite o arquivo `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ChallengeDB;Trusted_Connection=True;TrustServerCertificate=True;"
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
   
   *Se necessário, instale a ferramenta EF Core globalmente:*
   ```bash
   dotnet tool install --global dotnet-ef
   ```

5. **Compile o projeto**
   ```bash
   dotnet build
   ```

6. **Execute a aplicação**
   ```bash
   dotnet run
   ```

7. **Acesse a documentação Swagger**
   ```
   https://localhost:5001/swagger
   ```

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

**Resposta:**
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "userType": 1,
  "links": [
    {
      "rel": "self",
      "href": "/api/users/1",
      "method": "GET"
    }
  ]
}
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

### Consultar Histórico de um Veículo Específico

```bash
curl -X GET https://localhost:5001/api/maintenancehistories/vehicle/1
```

## 🧪 Testes

### Como executar os testes

Os testes estão localizados na pasta `Challenge.Tests/` e cobrem controllers, serviços e repositórios principais.

**Executar todos os testes:**
```bash
dotnet test Challenge.Tests/Challenge.Tests.csproj
```

**Executar testes com saída detalhada:**
```bash
dotnet test Challenge.Tests/Challenge.Tests.csproj --logger "console;verbosity=detailed"
```

**Executar testes de uma classe específica:**
```bash
dotnet test Challenge.Tests/Challenge.Tests.csproj --filter "FullyQualifiedName~UserControllerTests"
```

**Gerar relatório de cobertura de código:**
```bash
dotnet test Challenge.Tests/Challenge.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

**Estrutura dos arquivos de teste:**
```
Challenge.Tests/
├── MaintenanceHistoriesControllerTests.cs
├── UserControllerTests.cs
├── VehiclesControllerTests.cs
└── ...
```

*Certifique-se de restaurar as dependências e compilar o projeto antes de rodar os testes.*

## 📊 Benefícios e Ganhos

### Ganhos Operacionais
- **Centralização de dados**: Todas as informações em um único sistema
- **Rastreabilidade completa**: Histórico detalhado de todas as operações
- **Redução de erros**: Validações automáticas e dados estruturados
- **Tomada de decisão**: Relatórios e métricas para análises estratégicas

### Benefícios Técnicos
- **Manutenibilidade**: Código organizado e bem estruturado
- **Escalabilidade**: Arquitetura preparada para crescimento
- **Extensibilidade**: Fácil adição de novas funcionalidades
- **Testabilidade**: Separação de camadas facilita testes

## 👨‍💻 Equipe de Desenvolvimento

| Desenvolvedor | RM |
|---------------|-------|
| **Gustavo Lazzuri** | 556772 |
| **Eduardo Nagado** | 558158 |
| **Felipe Silva** | 555307 |

## 🤝 Como Contribuir

Contribuições são sempre bem-vindas! Para contribuir com o projeto:

1. Faça um fork do repositório
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

### Diretrizes de Contribuição

- Mantenha o código limpo e bem documentado
- Siga os padrões de código estabelecidos no projeto
- Adicione testes para novas funcionalidades
- Atualize a documentação quando necessário
- Descreva claramente as mudanças no Pull Request
- Certifique-se de que todos os testes estão passando

## 📞 Suporte e Contato

Para dúvidas técnicas, sugestões ou relatos de bugs:
- **Issues**: [GitHub Issues](https://github.com/seu-usuario/challenge-dotnet/issues)
- **Documentação**: Swagger UI integrada no projeto
- **Email**: gulazzuri@gmail.com

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

*Desenvolvido com ❤️ usando .NET 8 - Transformando a gestão de veículos através da tecnologia* 🚀
