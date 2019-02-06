using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public interface IAuthorizerApi : IApiBase
    {
        Task<AuthorizeResponseParameters> AuthorizeAsync(
            IYield yield,
            AuthorizeRequestParameters parameters);
    }
}