using System.Threading.Tasks;
using CommonTools.Coroutines;
using ComponentModel.Common;
using Game.Common;

namespace Character.Server.Common
{
    public interface ICharacterServiceAPI : IExposableComponent
    {
        Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParametersEx parameters);
        Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParametersEx parameters);
        Task<GetCharactersResponseParameters> GetCharacters(IYield yield, GetCharactersRequestParameters parameters);
        Task<GetCharacterResponseParameters> GetCharacter(IYield yield, GetCharacterRequestParametersEx parameters);
    }
}