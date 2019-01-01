using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public interface ICharacterPeerLogicAPI : IPeerLogicBase
    {
        Task<GetCharactersResponseParameters> GetCharacters(IYield yield);
        Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters);
        Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters);
        Task<ValidateCharacterResponseParameters> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters);
    }
}