using Characters.Client.Common;
using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterGetter : IExposableComponent
    {
        CharacterFromDatabaseParameters? GetCharacter(int userId, int characterIndex);
    }
}