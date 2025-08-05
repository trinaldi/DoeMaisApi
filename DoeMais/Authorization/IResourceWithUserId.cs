namespace DoeMais.Authorization;

public interface IResourceWithUserId
{
    long UserId { get; }
}