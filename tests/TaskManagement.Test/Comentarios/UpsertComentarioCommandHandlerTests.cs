namespace TaskManagement.Test.Comentarios;

public class UpsertComentarioCommandHandlerTests
{
    private readonly Mock<IRepository<ComentarioEntity>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHistoricoAtualizacaoService> _serviceMock;
    private readonly UpsertComentarioCommandHandler _handler;

    public UpsertComentarioCommandHandlerTests()
    {
        _repositoryMock = new Mock<IRepository<ComentarioEntity>>();
        _mapperMock = new Mock<IMapper>();
        _serviceMock = new Mock<IHistoricoAtualizacaoService>();
        _handler = new UpsertComentarioCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _serviceMock.Object);
    }

    [Fact(DisplayName = "Deve criar um comentário com sucesso")]
    public async Task DeveCriarComentarioComSucesso()
    {
        // Arrange
        var command = new UpsertComentarioCommand
        {
            Id = Guid.Empty,
            TarefaId = Guid.NewGuid(),
            Comentario = "Novo comentário"
        };

        var comentarioEntity = new ComentarioEntity
        {
            Id = Guid.NewGuid(),
            TarefaId = command.TarefaId,
            Comentario = command.Comentario
        };

        _mapperMock.Setup(mapper => mapper.Map<ComentarioEntity>(It.IsAny<UpsertComentarioCommand>()))
            .Returns(comentarioEntity);

        _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ComentarioEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(comentarioEntity.Id);

        _serviceMock.Setup(service => service.RegistrarHistoricoAsync(
                It.IsAny<TarefaEntity>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }


    [Fact(DisplayName = "Deve atualizar um comentário com sucesso")]
    public async Task DeveAtualizarComentarioComSucesso()
    {
        // Arrange
        var command = new UpsertComentarioCommand
        {
            Id = Guid.NewGuid(),
            TarefaId = Guid.NewGuid(),
            Comentario = "Comentário atualizado"
        };

        var comentarioEntity = new ComentarioEntity
        {
            Id = command.Id,
            TarefaId = command.TarefaId,
            Comentario = command.Comentario
        };

        _mapperMock.Setup(mapper => mapper.Map<ComentarioEntity>(It.IsAny<UpsertComentarioCommand>()))
            .Returns(comentarioEntity);

        _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(comentarioEntity);

        _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ComentarioEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _serviceMock.Setup(service => service.RegistrarHistoricoAsync(
                It.IsAny<TarefaEntity>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
    }

    [Fact(DisplayName = "Deve retornar erro em caso de exceção")]
    public async Task DeveRetornarErroEmCasoDeExcecao()
    {
        // Arrange
        var command = new UpsertComentarioCommand
        {
            Id = Guid.Empty,
            TarefaId = Guid.NewGuid(),
            Comentario = "Comentário com erro"
        };

        _mapperMock.Setup(mapper => mapper.Map<ComentarioEntity>(It.IsAny<UpsertComentarioCommand>()))
            .Throws(new Exception("Erro inesperado"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro inesperado", result.Message);
    }
}