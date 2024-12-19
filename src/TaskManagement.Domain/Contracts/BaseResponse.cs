namespace TaskManagement.Domain.Contracts;

public class BaseResponse<T>(T? result, bool success = true, string message = "") : IBaseResponse
{
    public T? Result { get; set; } = result;
    public bool Success { get; set; } = success;
    public string Message { get; set; } = message;
}