namespace TaskManagement.Domain.Enums;

public enum OperacaoCrud
{
    [Description("CRIAÇÃO")]
    Create = 1,

    [Description("LEITURA")]
    Read = 2,

    [Description("ATUALIZAÇÃO")]
    Update = 3,

    [Description("EXCLUSÃO")]
    Delete = 4
}