using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Services
{
    public interface ICharacterServiceAPI : IPeerLogicBase
    {
        UnityEvent<GetCharactersResponseParameters> ReceivedCharacters { get; }

        Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters);
        Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters);
        Task<CharacterValidationStatus> ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters);
    }
}