# api-tasks
API Tasks


- src/
  - Application/      -> Cont�m casos de uso (Handlers, Commands, Queries)
  - Domain/           -> Cont�m entidades e regras de neg�cio
  - Infrastructure/   -> Cont�m persist�ncia e implementa��es t�cnicas (EF Core, NLog, etc.)
  - Presentation/     -> Cont�m controllers e mapeamento de endpoints
- tests/
  - UnitTests/        -> Testes unit�rios
  - IntegrationTests/ -> Testes de integra��o (opcional nesta fase)
- docker-compose.yml  -> Arquivo para subir a API e o SQL Server no Docker
- Dockerfile          -> Arquivo Docker para a API
- README.md           -> Instru��es do projeto
