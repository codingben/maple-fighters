using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IAuthorizationServiceAPI : IPeerLogicBase
    {
        Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);
    }
}