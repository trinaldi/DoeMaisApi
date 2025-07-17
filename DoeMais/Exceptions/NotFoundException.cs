namespace DoeMais.Exceptions;

public class NotFoundException<T> : Exception
{
    public NotFoundException() : base($"{typeof(T).Name} not found.")
    {
    }
}