namespace TaskManagement.Test.Tarefas;

public class UpsertTarefaCommandHandlerTests
{
    private readonly Mock<IRepository<TarefaEntity>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHistoricoAtualizacaoService> _serviceMock;
    private readonly UpsertTarefaCommandHandler _handler;
    private const int MaxResults = 20;

    public UpsertTarefaCommandHandlerTests()
    {
        _repositoryMock = new Mock<IRepository<TarefaEntity>>();
        _mapperMock = new Mock<IMapper>();
        _serviceMock = new Mock<IHistoricoAtualizacaoService>();
        _handler = new UpsertTarefaCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _serviceMock.Object);
    }

    [Fact(DisplayName = "Deve criar uma nova tarefa com sucesso")]
    public async Task DeveCriarNovaTarefaComSucesso()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var command = new UpsertTarefaCommand
        {
            Id = Guid.Empty,
            ProjetoId = projetoId,
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da Tarefa",
            Prioridade = PrioridadeTarefa.Alta
        };
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto com Tarefa 1",
            Descricao = "Projeto Tarefa 1"
        };

        var tarefaEntity = new TarefaEntity
        {
            Id = Guid.NewGuid(),
            ProjetoId = projetoId,
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da Tarefa",
            Prioridade = PrioridadeTarefa.Alta,
            Projeto = projeto
        };

        _mapperMock.Setup(mapper => mapper.Map<TarefaEntity>(It.IsAny<UpsertTarefaCommand>()))
            .Returns(tarefaEntity);

        _repositoryMock.Setup(repo => repo.GetSpecificAsync(It.IsAny<Expression<Func<TarefaEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<TarefaEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        _serviceMock.Setup(service => service.RegistrarHistoricoAsync(
                It.IsAny<TarefaEntity>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(string.Empty, result.Message);
    }

    [Fact(DisplayName = "Deve atualizar uma tarefa existente com sucesso")]
    public async Task DeveAtualizarTarefaExistenteComSucesso()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto com Tarefa 1",
            Descricao = "Projeto Tarefa 1"
        };
        var command = new UpsertTarefaCommand
        {
            Id = tarefaId,
            ProjetoId = projetoId,
            Titulo = "Tarefa Atualizada",
            Descricao = "Descrição Atualizada",
            Prioridade = PrioridadeTarefa.Alta
        };

        var tarefaEntity = new TarefaEntity
        {
            Id = tarefaId,
            ProjetoId = projetoId,
            Titulo = "Tarefa Original",
            Descricao = "Descrição Original",
            Prioridade = PrioridadeTarefa.Alta,
            Projeto = projeto
        };

        _mapperMock.Setup(mapper => mapper.Map<TarefaEntity>(It.IsAny<UpsertTarefaCommand>()))
            .Returns(tarefaEntity);

        _repositoryMock.Setup(repo => repo.GetByIdAsync(tarefaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefaEntity);

        _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<TarefaEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _serviceMock.Setup(service => service.RegistrarHistoricoAsync(
                It.IsAny<TarefaEntity>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(string.Empty, result.Message);
    }

    [Fact(DisplayName = "Deve falhar ao criar tarefa quando o limite máximo é atingido")]
    public async Task DeveFalharQuandoLimiteMaximoDeTarefasEhAtingido()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto com Tarefa 1",
            Descricao = "Projeto Tarefa 1"
        };

        var command = new UpsertTarefaCommand
        {
            Id = Guid.Empty,
            ProjetoId = projetoId,
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da Tarefa",
            Prioridade = PrioridadeTarefa.Alta
        };

        var tarefasExistentes = Enumerable.Range(1, MaxResults).Select(i => new TarefaEntity
        {
            Id = Guid.NewGuid(),
            ProjetoId = projetoId,
            Titulo = $"Tarefa {i}",
            Descricao = $"Descrição {i}",
            Prioridade = PrioridadeTarefa.Media,
            Projeto = projeto
        }).ToList();

        _repositoryMock.Setup(repo => repo.GetSpecificAsync(It.IsAny<Expression<Func<TarefaEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefasExistentes);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Número de Tarefas maior que o permitido: '{MaxResults}'", result.Message);
    }
}
