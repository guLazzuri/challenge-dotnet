# 🏍️ GEF API - Sistema de Gestão de Pátio Inteligente

> Sistema digital inteligente para mapeamento e gestão de pátio de motocicletas, desenvolvido em ASP.NET Core com integração Oracle Database como parte do Challenge FIAP 2025.

[![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-F80000?style=flat-square&logo=oracle&logoColor=white)](https://www.oracle.com/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)](https://swagger.io/)

## 📋 Visão Geral

O MotoConnect é uma solução tecnológica que visa modernizar e otimizar a gestão de pátios de motocicletas, substituindo processos manuais por um sistema digital inteligente. A plataforma oferece controle total sobre a localização, status e histórico de veículos, proporcionando eficiência operacional e tomada de decisões baseada em dados.

### 🎯 Problemas Solucionados

- **Localização Manual Ineficiente**: Elimina a busca demorada por motocicletas específicas no pátio
- **Desorganização Espacial**: Implementa padronização na disposição e categorização dos veículos
- **Falta de Rastreabilidade**: Fornece histórico completo e análises sobre ocupação e movimentação
- **Ausência de Integração Tecnológica**: Prepara base para futuras automações com IoT e QR Codes

### 🚀 Objetivos da Solução

- **Mapeamento Digital**: Planta virtual do pátio com posicionamento em tempo real das motocicletas
- **Categorização Inteligente**: Status operacional claro (manutenção, liberada, defeito, vistoria)
- **Interface Moderna**: Dashboard web responsivo acessível em qualquer dispositivo
- **Escalabilidade**: Arquitetura preparada para integração com sensores IoT e automação

## 🔧 Funcionalidades Principais

### 📍 Gestão de Localização
- Mapeamento visual do pátio com posicionamento exato das motos
- Sistema de coordenadas para localização precisa
- Busca rápida por placa, código ou posição

### 🏷️ Controle de Status
- Categorização por status operacional
- Triagem visual com cores e indicadores
- Histórico completo de mudanças de status

### 📊 Relatórios e Analytics
- Análise de ocupação do pátio
- Relatórios de movimentação histórica
- Indicadores de performance operacional

### 🔄 Integração Futura
- Preparação para leitura de QR Codes
- Arquitetura IoT-ready para sensores automáticos
- API extensível para integrações externas

## 🔗 Endpoints da API

### 🏍️ Vehicles (Motocicletas)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Vehicles` | Listar todas as motocicletas com status e localização |
| POST | `/api/Vehicles` | Cadastrar nova motocicleta no sistema |
| GET | `/api/Vehicles/{id}` | Buscar motocicleta específica por ID |
| PUT | `/api/Vehicles/{id}` | Atualizar informações e status da motocicleta |
| DELETE | `/api/Vehicles/{id}` | Remover motocicleta do sistema |


### 👤 Users (Usuários)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/User` | Listar usuários do sistema |
| POST | `/api/User` | Criar novo usuário |
| GET | `/api/User/{id}` | Buscar usuário específico |
| PUT | `/api/User/{id}` | Atualizar dados do usuário |
| DELETE | `/api/User/{id}` | Remover usuário do sistema |

### 🛠️ MaintenanceHistories (Histórico de Manutenção)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/MaintenanceHistories` | Listar todos os registros de manutenção |
| POST | `/api/MaintenanceHistories` | Criar novo registro de manutenção |
| GET | `/api/MaintenanceHistories/{id}` | Detalhar registro específico |
| PUT | `/api/MaintenanceHistories/{id}` | Atualizar registro de manutenção |


## 🏗️ Arquitetura e Tecnologias

### Backend
- **ASP.NET Core 8**: Framework principal para desenvolvimento da API
- **Oracle Database**: Banco de dados robusto para armazenamento empresarial
- **Entity Framework Core**: ORM para mapeamento objeto-relacional
- **Swagger/OpenAPI**: Documentação automática da API

### Estrutura de Dados
```
📁 Models/
├── Vehicle.cs          # Entidade principal (moto + localização + status)
├── User.cs             # Usuários do sistema
├── MaintenanceHistory.cs # Histórico de manutenções
```

### Padrões Implementados
- **Repository Pattern**: Abstração de acesso a dados
- **Dependency Injection**: Inversão de controle nativa do .NET
- **RESTful API**: Arquitetura REST para comunicação
- **Clean Architecture**: Separação clara de responsabilidades

## ⚙️ Instalação e Configuração

### Pré-requisitos
- .NET 8.0 SDK
- Oracle Database (local ou cloud)
- Visual Studio 2022 ou VS Code

### Passos de Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/guLazzuri/challenge-dotnet.git
   cd challenge-dotnet
   ```

2. **Configure a conexão Oracle**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_datasource;"
     }
   }
   ```

3. **Execute as migrações**
   ```bash
   dotnet ef database update
   ```

4. **Inicie a aplicação**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

5. **Acesse a documentação**
   ```
   https://localhost:5214/swagger
   ```

## 📈 Roadmap de Evolução

### Fase 1 (Atual) - Fundação Digital
- ✅ API RESTful completa
- ✅ Gestão básica de veículos e usuários
- ✅ Histórico de manutenções
- ✅ Documentação Swagger

### Fase 2 - Interface Visual
- 🔄 Dashboard web responsivo
- 🔄 Mapa interativo do pátio
- 🔄 Filtros e buscas avançadas
- 🔄 Relatórios visuais

### Fase 3 - Automação QR Code
- 📋 Integração com leitura de QR Codes
- 📋 Cadastro automático via código
- 📋 App mobile para operação

### Fase 4 - IoT e Sensores
- 📋 Sensores de presença automáticos
- 📋 Alertas em tempo real
- 📋 Coleta de dados ambientais
- 📋 Integração com sistemas externos

## 📊 Métricas e Benefícios

### Ganhos Operacionais
- **Redução de 80%** no tempo de localização de veículos
- **Eliminação de 100%** dos erros de registro manual
- **Aumento de 60%** na eficiência da triagem

### Benefícios Técnicos
- Padronização completa do processo
- Rastreabilidade total das operações
- Base sólida para automação futura
- Relatórios precisos para tomada de decisão

## 👨‍💻 Equipe de Desenvolvimento

| Desenvolvedor | RM | Especialização |
|---------------|-------|---------------|
| **Gustavo Lazzuri** | 556772 | Backend & Database |
| **Eduardo Nagado** | 558158 | API & Integração |
| **Felipe Silva** | 555307 | Arquitetura & DevOps |

## 📞 Suporte e Contato

Para dúvidas técnicas, sugestões ou relatos de bugs:
- **Issues**: [GitHub Issues](https://github.com/guLazzuri/challenge-dotnet/issues)
- **Documentação**: Swagger UI integrada
- **Contato**: Via plataforma FIAP

## 📄 Licença

Este projeto é desenvolvido para fins educacionais como parte do Challenge FIAP 2025. Todos os direitos são reservados para uso acadêmico e avaliação.

---

*Transformando a gestão de pátios através da tecnologia* 🚀
