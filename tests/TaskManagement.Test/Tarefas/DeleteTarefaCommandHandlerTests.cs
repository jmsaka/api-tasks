namespace TaskManagement.Test.Tarefas;

public class DeleteTarefaCommandHandlerTests
{
    private readonly Mock<IRepository<TarefaEntity>> _tarefaRepositoryMock;
    private readonly Mock<IHistoricoAtualizacaoService> _serviceMock;
    private readonly DeleteTarefaCommandHandler _handler;

    public DeleteTarefaCommandHandlerTests()
    {
        _tarefaRepositoryMock = new Mock<IRepository<TarefaEntity>>();
        _serviceMock = new Mock<IHistoricoAtualizacaoService>();
        _handler = new DeleteTarefaCommandHandler(_tarefaRepositoryMock.Object, _serviceMock.Object);
    }

    [Fact(DisplayName = "Não permitir exclusão de Tarefa com Comentários")]
    public async Task TestNaoDeletarTarefaComComentarios()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto com Tarefa 1",
            Descricao = "Projeto Tarefa 1"
        };

        var tarefaId = Guid.NewGuid();

        List<ComentarioEntity> comentarios =
        [
            new() { Id = Guid.NewGuid(), Comentario = "Comentário A", DataCriacao = DateTime.Now, TarefaId = tarefaId },
            new() { Id = Guid.NewGuid(), Comentario = "Comentário B", DataCriacao = DateTime.Now.AddMinutes(-10), TarefaId = tarefaId },
            new() { Id = Guid.NewGuid(), Comentario = "Comentário C", DataCriacao = DateTime.Now.AddMinutes(-20), TarefaId = tarefaId }
        ];

        var tarefa = new TarefaEntity
        {
            Id = tarefaId,
            Titulo = "Tarefa com Comentários",
            Descricao = "Tarefa com comentários associados",
            Status = StatusTarefa.Pendente,
            Projeto = projeto,
            Comentarios = comentarios
        };

        _tarefaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefa);

        _tarefaRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new DeleteTarefaCommand { Id = tarefaId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Não é possível deletar a Tarefa. Existem comentarios associados.", result.Message);
        _tarefaRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Permitir exclusão de Tarefa sem Comentários")]
    public async Task TestDeletarTarefaSemComentarios()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto com Tarefa 1",
            Descricao = "Projeto Tarefa 1"
        };

        var tarefaId = Guid.NewGuid();
        var tarefa = new TarefaEntity
        {
            Id = tarefaId,
            Titulo = "Tarefa sem Comentários",
            Descricao = "Tarefa sem comentários associados",
            Status = StatusTarefa.Pendente,
            Projeto = projeto
        };

        _tarefaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefa);

        _tarefaRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _serviceMock.Setup(service => service.RegistrarHistoricoAsync(
                It.IsAny<TarefaEntity>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new DeleteTarefaCommand { Id = tarefaId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(string.Empty, result.Message);
        _tarefaRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}