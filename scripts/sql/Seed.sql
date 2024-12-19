-- Criando tabelas tempor�rias para armazenar os IDs dos Projetos e Tarefas
DECLARE @Projetos TABLE (Id UNIQUEIDENTIFIER, Nome NVARCHAR(100));
DECLARE @Tarefas TABLE (Id UNIQUEIDENTIFIER, ProjetoId UNIQUEIDENTIFIER, Titulo NVARCHAR(100));

-- ==============================
-- Inser��o de Projetos
-- ==============================
INSERT INTO Projetos (Id, Nome, Descricao, DataCriacao)
OUTPUT INSERTED.Id, INSERTED.Nome INTO @Projetos(Id, Nome)
VALUES
(NEWID(), 'Desenvolvimento de Sistema de Gest�o', 'Projeto para desenvolver um sistema de gest�o para a empresa de TI', GETUTCDATE()),
(NEWID(), 'Implementa��o de API Restful', 'Projeto para criar e implementar uma API Restful utilizando .NET Core', GETUTCDATE()),
(NEWID(), 'Automa��o de Testes de Software', 'Projeto de automa��o de testes para melhorar a cobertura de testes', GETUTCDATE()),
(NEWID(), 'Desenvolvimento de Aplicativo Mobile', 'Projeto para desenvolver um aplicativo mobile para iOS e Android', GETUTCDATE()),
(NEWID(), 'Integra��o de Sistemas ERP', 'Projeto para integrar diferentes m�dulos de um sistema ERP', GETUTCDATE());

-- ==============================
-- Inser��o de Tarefas Din�micas
-- ==============================
DECLARE @ProjetoId UNIQUEIDENTIFIER;

DECLARE ProjetoCursor CURSOR FOR
SELECT Id FROM @Projetos;

OPEN ProjetoCursor;
FETCH NEXT FROM ProjetoCursor INTO @ProjetoId;

WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO Tarefas (Id, Titulo, Descricao, DataVencimento, Status, Prioridade, ProjetoId)
    OUTPUT INSERTED.Id, INSERTED.ProjetoId, INSERTED.Titulo INTO @Tarefas(Id, ProjetoId, Titulo)
    VALUES
    (NEWID(), 'An�lise de Requisitos', 'Realizar levantamento de requisitos com o cliente', DATEADD(DAY, 7, GETUTCDATE()), 'Pendente', 'Alta', @ProjetoId),
    (NEWID(), 'Desenvolvimento Backend', 'Desenvolver API para cadastro de usu�rios', DATEADD(DAY, 14, GETUTCDATE()), 'Pendente', 'M�dia', @ProjetoId),
    (NEWID(), 'Testes de Unidade', 'Escrever testes unit�rios para a API', DATEADD(DAY, 10, GETUTCDATE()), 'Pendente', 'M�dia', @ProjetoId),
    (NEWID(), 'Revis�o de C�digo', 'Revisar o c�digo desenvolvido para garantir qualidade', DATEADD(DAY, 5, GETUTCDATE()), 'Pendente', 'Alta', @ProjetoId),
    (NEWID(), 'Desenvolvimento Frontend', 'Desenvolver a interface do usu�rio', DATEADD(DAY, 7, GETUTCDATE()), 'Pendente', 'M�dia', @ProjetoId);

    FETCH NEXT FROM ProjetoCursor INTO @ProjetoId;
END;

CLOSE ProjetoCursor;
DEALLOCATE ProjetoCursor;

-- ==============================
-- Inser��o de Coment�rios Din�micos
-- ==============================
DECLARE @TarefaId UNIQUEIDENTIFIER;
DECLARE @RandomComments TABLE (Comentario NVARCHAR(255));
INSERT INTO @RandomComments VALUES
('�timo progresso at� agora!'),
('Revisar a documenta��o antes de prosseguir.'),
('Aten��o aos prazos de entrega.'),
('Discuss�o pendente com o cliente sobre este item.');

DECLARE TarefaCursor CURSOR FOR
SELECT Id FROM @Tarefas;

OPEN TarefaCursor;
FETCH NEXT FROM TarefaCursor INTO @TarefaId;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @CommentIndex INT = 1;

    WHILE @CommentIndex <= 3 -- Gera de 1 a 3 coment�rios por tarefa
    BEGIN
        INSERT INTO Comentarios (Id, TarefaId, Comentario, DataCriacao)
        SELECT NEWID(), @TarefaId, Comentario, GETUTCDATE()
        FROM @RandomComments
        WHERE Comentario = (SELECT TOP 1 Comentario FROM @RandomComments ORDER BY NEWID());

        SET @CommentIndex = @CommentIndex + 1;
    END;

    FETCH NEXT FROM TarefaCursor INTO @TarefaId;
END;

CLOSE TarefaCursor;
DEALLOCATE TarefaCursor;

-- ==============================
-- Resultado Final
-- ==============================
SELECT * FROM Projetos;
SELECT * FROM Tarefas;
SELECT * FROM Comentarios;
