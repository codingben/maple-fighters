using Scripts.Network.APIs;

namespace Scripts.Network.Services
{
    public interface IAuthenticatorService
    {
        IAuthenticatorApi GetAuthenticatorApi();
    }
}