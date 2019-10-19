using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;

namespace Scripts.Services.Authorizer
{
    public interface IAuthorizerApi
    {
        Task<AuthorizeResponseParameters> AuthorizeAsync(IYield yield, AuthorizeRequestParameters parameters);
    }
}