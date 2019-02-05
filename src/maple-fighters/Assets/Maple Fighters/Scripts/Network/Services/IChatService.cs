namespace Scripts.Services
{
    public interface IChatService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IChatApi GetChatApi();
    }
}