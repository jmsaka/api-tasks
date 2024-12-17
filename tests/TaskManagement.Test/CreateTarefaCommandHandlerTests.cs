using AutoMapper;
using Moq;
using TaskManagement.Application.Commands.Tarefas;
using TaskManagement.Application.Profiles;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using Xunit;

namespace TaskManagement.Test;

public class CreateTarefaCommandHandlerTests
{
    private readonly Mock<IRepository<ProjetoEntity>> _mockProjetoRepository;
    private readonly Mock<IRepository<TarefaEntity>> _mockTarefaRepository;
    private readonly IMapper _mapper;

    public CreateTarefaCommandHandlerTests()
    {
        // Configuração dos mocks
        _mockProjetoRepository = new Mock<IRepository<ProjetoEntity>>();
        _mockTarefaRepository = new Mock<IRepository<TarefaEntity>>();

        // Configuração do AutoMapper
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new TarefaProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnGuid()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        ProjetoEntity projeto = new ProjetoEntity
        {
            Id = projetoId,
            Tarefas = new List<TarefaEntity>()
        };

        // Configurar o mock do repositório de Projeto para retornar um projeto com suas tarefas
        _mockProjetoRepository
            .Setup(repo => repo.GetObjectWithAnotherAsync(projetoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(projeto);

        // Configurar o mock do repositório de Tarefa para AddAsync
        var tarefaId = Guid.NewGuid(); // Simula o ID retornado ao adicionar a tarefa

        _mockTarefaRepository
            .Setup(repo => repo.AddAsync(It.IsAny<TarefaEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefaId); // Retorna o Guid corretamente


        var handler = new CreateTarefaCommandHandler(
            _mockProjetoRepository.Object,
            _mockTarefaRepository.Object,
            _mapper
        );

        var command = new CreateTarefaCommand
        {
            ProjetoId = projetoId,
            Titulo = "Nova Tarefa",
            Descricao = "Descrição de teste",
            DataVencimento = DateTime.UtcNow.AddDays(1),
            Prioridade = "Alta"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<Guid>(result);
        _mockProjetoRepository.Verify(repo => repo.GetObjectWithAnotherAsync(projetoId, It.IsAny<CancellationToken>()), Times.Once);
        _mockProjetoRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
