# api-tasks
API Tasks


- src/
  - Application/      -> Contém casos de uso (Handlers, Commands, Queries)
  - Domain/           -> Contém entidades e regras de negócio
  - Infrastructure/   -> Contém persistência e implementações técnicas (EF Core, NLog, etc.)
  - Presentation/     -> Contém controllers e mapeamento de endpoints
- tests/
  - UnitTests/        -> Testes unitários
  - IntegrationTests/ -> Testes de integração (opcional nesta fase)
- docker-compose.yml  -> Arquivo para subir a API e o SQL Server no Docker
- Dockerfile          -> Arquivo Docker para a API
- README.md           -> Instruções do projeto
