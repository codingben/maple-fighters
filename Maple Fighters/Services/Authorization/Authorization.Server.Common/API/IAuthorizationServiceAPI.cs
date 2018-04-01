using System.Threading.Tasks;
using CommonTools.Coroutines;

namespace Authorization.Server.Common
{
    public interface IAuthorizationServiceAPI
    {
        void RemoveAuthorization(RemoveAuthorizationRequestParameters parameters);
        Task<AuthorizeAccessTokenResponseParameters> AccessTokenAuthorization(IYield yield, AuthorizeAccesTokenRequestParameters parameters);
        Task<AuthorizeUserResponseParameters> UserAuthorization(IYield yield, AuthorizeUserRequestParameters parameters);
    }
}