namespace Scripts.Services
{
    public interface IChatService : IServiceBase
    {
        IAuthorizerApi AuthorizerApi { get; }

        IChatApi ChatApi { get; }
    }
}