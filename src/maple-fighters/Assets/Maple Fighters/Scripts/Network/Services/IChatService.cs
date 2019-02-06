namespace Scripts.Network
{
    public interface IChatService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IChatApi GetChatApi();
    }
}