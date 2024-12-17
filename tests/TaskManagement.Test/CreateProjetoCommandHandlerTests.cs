using AutoMapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Commands.Projetos;
using TaskManagement.Application.Profiles;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using Xunit;

namespace TaskManagement.Test
{
    public class CreateProjetoCommandHandlerTests
    {
        private readonly Mock<IRepository<ProjetoEntity>> _mockRepository;
        private readonly IMapper _mapper;

        public CreateProjetoCommandHandlerTests()
        {
            // Mock do reposit�rio gen�rico
            _mockRepository = new Mock<IRepository<ProjetoEntity>>();

            // Configura��o do AutoMapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new ProjetoProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnGuid()
        {
            // Arrange
            var command = new CreateProjetoCommand
            {
                Nome = "Projeto Teste",
                Descricao = "Descri��o Teste"
            };

            var projetoId = Guid.NewGuid();

            // Configurar o mock para o m�todo AddAsync retornar um Guid
            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projetoId);

            var handler = new CreateProjetoCommandHandler(_mockRepository.Object, _mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(projetoId, result);

            // Verificar se o m�todo AddAsync foi chamado exatamente uma vez
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}