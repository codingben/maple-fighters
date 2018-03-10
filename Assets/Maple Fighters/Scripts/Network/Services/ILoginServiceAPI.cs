using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;

namespace Scripts.Services
{
    public interface ILoginServiceAPI : IServiceBase
    {
        Task<AuthenticateResponseParameters> Authenticate(IYield yield, AuthenticateRequestParameters parameters);
    }
}