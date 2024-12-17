using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Contracts;

public class BaseResponse<T> : IBaseResponse
{
    public T? Result { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public BaseResponse(T? result, bool success = true, string message = "")
    {
        Result = result;
        Success = success;
        Message = message;
    }
}