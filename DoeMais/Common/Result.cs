using DoeMais.Domain.Enums;

namespace DoeMais.Common;

public class Result<T>
{
    public ResultType Type { get; }
    public string? Message { get; }
    public T? Data { get; }

    public Result(ResultType type, string? message, T? data = default)
    {
        Type = type;
        Message = message;
        Data = data;
    }
}