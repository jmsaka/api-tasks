namespace TaskManagement.CrossCutting.Helpers;

public static class EnumHelper
{
    public static string GetEnumDescription<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }

    public static TEnum GetEnumFromDescription<TEnum>(string description) where TEnum : struct, Enum
    {
        foreach (var field in typeof(TEnum).GetFields())
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute?.Description == description)
            {
                return (TEnum)Enum.Parse(typeof(TEnum), field.Name);
            }
        }
        throw new ArgumentException($"Descrição '{description}' não encontrada em {typeof(TEnum)}");
    }
}