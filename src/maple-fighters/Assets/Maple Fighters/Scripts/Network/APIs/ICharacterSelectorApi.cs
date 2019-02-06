using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;

namespace Scripts.Network.APIs
{
    public interface ICharacterSelectorApi : IApiBase
    {
        Task<CreateCharacterResponseParameters> CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters);

        Task<RemoveCharacterResponseParameters> RemoveCharacterAsync(
            IYield yield,
            RemoveCharacterRequestParameters parameters);

        Task<ValidateCharacterResponseParameters> ValidateCharacterAsync(
            IYield yield,
            ValidateCharacterRequestParameters parameters);

        Task<GetCharactersResponseParameters> GetCharactersAsync(
            IYield yield);
    }
}