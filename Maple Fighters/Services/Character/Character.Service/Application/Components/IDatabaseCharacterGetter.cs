using Character.Client.Common;
using ComponentModel.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterGetter : IExposableComponent
    {
        CharacterFromDatabaseParameters? GetCharacter(int userId, int characterIndex);
    }
}