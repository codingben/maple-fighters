using System.Threading.Tasks;
using Authorization.Client.Common;
using Character.Client.Common;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface ICharacterService : IServiceBase
    {
        Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);
        Task<GetCharactersResponseParameters> GetCharacters(IYield yield);
        Task<CreateCharacterResponseParameters> CreateCharacter(IYield yield, CreateCharacterRequestParameters parameters);
        Task<RemoveCharacterResponseParameters> RemoveCharacter(IYield yield, RemoveCharacterRequestParameters parameters);
    }
}