using System.Threading.Tasks;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace Character.Server.Common
{
    public interface ICharacterServiceAPI : IExposableComponent
    {
        Task<GetCharacterResponseParameters> GetCharacter(IYield yield, GetCharacterRequestParameters parameters);
    }
}