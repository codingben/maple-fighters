using System.Threading.Tasks;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace Authorization.Server.Common
{
    public interface IAuthorizationServiceAPI : IExposableComponent
    {
        void RemoveAuthorization(RemoveAuthorizationRequestParameters parameters);
        Task<AuthorizeAccessTokenResponseParameters> AccessTokenAuthorization(IYield yield, AuthorizeAccesTokenRequestParameters parameters);
        Task<AuthorizeUserResponseParameters> UserAuthorization(IYield yield, AuthorizeUserRequestParameters parameters);
    }
}