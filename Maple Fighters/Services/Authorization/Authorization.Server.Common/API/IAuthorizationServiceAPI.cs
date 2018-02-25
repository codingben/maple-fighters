using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace Authorization.Server.Common
{
    public interface IAuthorizationServiceAPI : IExposableComponent
    {
        Task<CreateAuthorizationResponseParameters> CreateAuthorization(IYield yield, CreateAuthorizationRequestParameters parameters);
        Task<EmptyParameters> RemoveAuthorization(IYield yield, RemoveAuthorizationRequestParameters parameters);
        Task<AuthorizeAccessTokenResponseParameters> AccessTokenAuthorization(IYield yield, AuthorizeAccesTokenRequestParameters parameters);
        Task<AuthorizeUserResponseParameters> UserAuthorization(IYield yield, AuthorizeUserRequestParameters parameters);
    }
}