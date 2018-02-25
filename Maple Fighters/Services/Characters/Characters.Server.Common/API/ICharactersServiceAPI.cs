using System.Threading.Tasks;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace Characters.Server.Common
{
    public interface ICharactersServiceAPI : IExposableComponent
    {
        Task<GetCharacterResponseParameters> GetCharacter(IYield yield, GetCharacterRequestParameters parameters);
    }
}