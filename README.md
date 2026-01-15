# Basis-Backend

API REST desenvolvida em C# .NET Core 8.0 para gerenciamento de cadastro de livros, autores e assuntos. Este projeto foi desenvolvido como parte do teste técnico para a Empresa Basis.

## 📋 Sobre o Projeto

O **Basis-Backend** é uma API completa para cadastro e gerenciamento de livros, permitindo relacionar livros com múltiplos autores e assuntos. O sistema oferece operações CRUD completas para todas as entidades principais, com paginação, validações e tratamento de erros robusto.

### Principais Funcionalidades

- ✅ CRUD completo para Livros, Autores e Assuntos
- ✅ Relacionamentos muitos-para-muitos entre Livros ↔ Autores e Livros ↔ Assuntos
- ✅ Paginação de resultados
- ✅ Validação de dados de entrada
- ✅ Tratamento global de exceções
- ✅ Logging estruturado com NLog
- ✅ Observabilidade com OpenTelemetry
- ✅ Rate Limiting para proteção da API
- ✅ Health Checks para monitoramento (liveness e readness)
- ✅ Documentação automática com Swagger
- ✅ Testes unitários

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas (Layered Architecture) com separação clara de responsabilidades:

```
CadastroLivros.Api      → Camada de apresentação (Controllers, Middlewares, Configurações)
CadastroLivros.Core     → Camada de domínio (Entities, Services, DTOs, Contracts)
CadastroLivros.Infra    → Camada de infraestrutura (Repositories, DbContext, Mappers)
CadastroLivros.UnitTests → Testes unitários
```

### Princípios de Design

- **Separation of Concerns**: Cada camada tem responsabilidades bem definidas
- **Dependency Inversion**: Dependências apontam para abstrações

### Padrões Arquiteturais e de Projeto

- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Gerenciamento transacional
- **Service Layer**: Lógica de negócio isolada
- **Result Pattern (ErrorOr)**: Tratamento funcional de erros usando ErrorOr para evitar o lançamento de exceções e tornar os erros explícitos no código

