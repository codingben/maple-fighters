using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;

namespace Scripts.Services
{
    public interface ILoginServiceAPI : IPeerLogicBase
    {
        Task<AuthenticateResponseParameters> Authenticate(IYield yield, AuthenticateRequestParameters parameters);
    }
}