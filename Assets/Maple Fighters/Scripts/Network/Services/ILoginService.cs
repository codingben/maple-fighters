using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;

namespace Scripts.Services
{
    public interface ILoginService : IServiceBase
    {
        Task<LoginResponseParameters> Login(IYield yield, LoginRequestParameters parameters);
    }
}