namespace TaskManagement.Domain.Enums;

public enum StatusTarefa
{
    [Description("Pendente")]
    Pendente = 1,

    [Description("Em Andamento")]
    EmAndamento = 2,

    [Description("Concluída")]
    Concluida = 3
}