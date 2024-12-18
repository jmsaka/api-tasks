namespace TaskManagement.Test.Comentarios;

public class DeleteComentarioCommandHandlerTests
{
    private readonly Mock<IRepository<TarefaEntity>> _repositoryMock;
    private readonly Mock<IHistoricoAtualizacaoService> _serviceMock;
    private readonly DeleteComentarioCommandHandler _handler;

    public DeleteComentarioCommandHandlerTests()
    {
        _repositoryMock = new Mock<IRepository<TarefaEntity>>();
        _serviceMock = new Mock<IHistoricoAtualizacaoService>();
        _handler = new DeleteComentarioCommandHandler(_repositoryMock.Object, _serviceMock.Object);
    }

    [Fact(DisplayName = "Deve deletar um comentário com sucesso")]
    public async Task DeveDeletarComentarioComSucesso()
    {
        // Arrange
        var comentarioId = Guid.NewGuid();
        var tarefaId = Guid.NewGuid();

        var tarefaEntity = new TarefaEntity
        {
            Id = tarefaId,
            Titulo = "Tarefa 1",
            Descricao = "Tarefa 1",
            Projeto = new ProjetoEntity
            {
                Id = Guid.NewGuid(),
                Descricao = "Projeto 1",
                Nome = "Projeto 1",
                DataCriacao = DateTime.Now
            },
            Comentarios =
            [
                new ComentarioEntity { Id = comentarioId, Comentario = "Teste", DataCriacao = DateTime.UtcNow }
            ]
        };

        var command = new DeleteComentarioCommand
        {
            Id = comentarioId
        };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(tarefaEntity);

        _repositoryMock.Setup(repo => repo.DeleteAsync(comentarioId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true);

        _serviceMock.Setup(service => service.RegistrarHistoricoAsync(
                It.IsAny<TarefaEntity>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(string.Empty, result.Message);

        _repositoryMock.Verify(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(comentarioId, It.IsAny<CancellationToken>()), Times.Once);
        _serviceMock.Verify(service => service.RegistrarHistoricoAsync(
                                It.IsAny<TarefaEntity>(),
                                tarefaId,
                                "ADM",
                                EnumHelper.GetEnumDescription(OperacaoCrud.Delete),
                                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao tentar deletar um comentário inexistente")]
    public async Task DeveRetornarErroAoDeletarComentarioInexistente()
    {
        // Arrange
        var comentarioId = Guid.NewGuid();
        var command = new DeleteComentarioCommand { Id = comentarioId };

#nullable disable
        _repositoryMock.Setup(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()))
               .ReturnsAsync(null as TarefaEntity);
#nullable restore
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Comentário não encontrado.", result.Message);

        _repositoryMock.Verify(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _serviceMock.Verify(service => service.RegistrarHistoricoAsync(
                                It.IsAny<TarefaEntity>(),
                                It.IsAny<Guid>(),
                                It.IsAny<string>(),
                                It.IsAny<string>(),
                                It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Deve retornar erro ao falhar na exclusão do comentário")]
    public async Task DeveRetornarErroAoFalharNaExclusaoDoComentario()
    {
        // Arrange
        var comentarioId = Guid.NewGuid();
        var tarefaId = Guid.NewGuid();

        var tarefaEntity = new TarefaEntity
        {
            Id = tarefaId,
            Titulo = "Tarefa 1",
            Descricao = "Tarefa 1",
            Projeto = new ProjetoEntity
            {
                Id = Guid.NewGuid(),
                Descricao = "Projeto 1",
                Nome = "Projeto 1",
                DataCriacao = DateTime.Now
            },
            Comentarios =
            [
                new ComentarioEntity { Id = comentarioId, Comentario = "Teste", DataCriacao = DateTime.UtcNow }
            ]
        };

        var command = new DeleteComentarioCommand { Id = comentarioId };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(tarefaEntity);

        _repositoryMock.Setup(repo => repo.DeleteAsync(comentarioId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Falha ao deletar o Comentário.", result.Message);

        _repositoryMock.Verify(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(comentarioId, It.IsAny<CancellationToken>()), Times.Once);
        _serviceMock.Verify(service => service.RegistrarHistoricoAsync(
                                It.IsAny<TarefaEntity>(),
                                tarefaId,
                                "ADM",
                                EnumHelper.GetEnumDescription(OperacaoCrud.Delete),
                                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao lançar exceção durante o processo de exclusão")]
    public async Task DeveRetornarErroAoLancarExcecao()
    {
        // Arrange
        var comentarioId = Guid.NewGuid();
        var command = new DeleteComentarioCommand { Id = comentarioId };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("Erro interno"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Falha ao deletar o Comentário.", result.Message);

        _repositoryMock.Verify(repo => repo.GetByIdAsync(comentarioId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _serviceMock.Verify(service => service.RegistrarHistoricoAsync(
                                It.IsAny<TarefaEntity>(),
                                It.IsAny<Guid>(),
                                It.IsAny<string>(),
                                It.IsAny<string>(),
                                It.IsAny<CancellationToken>()), Times.Never);
    }
}