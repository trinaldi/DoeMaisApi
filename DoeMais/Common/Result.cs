using DoeMais.Domain.Enums;

namespace DoeMais.Common;

public class Result<T>
{
    public ResultType Type { get; }
    public string? Message { get; }
    public T? Data { get; }

    public Result(ResultType type, T? data = default, string? message = null)
    {
        Type = type;
        Data = data;
        Message = type == ResultType.Success ? null : message;
    }
}