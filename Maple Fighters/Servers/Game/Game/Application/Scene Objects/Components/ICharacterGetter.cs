using Characters.Client.Common;
using ComponentModel.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface ICharacterGetter : IExposableComponent
    {
        CharacterFromDatabaseParameters GetCharacter();
    }
}