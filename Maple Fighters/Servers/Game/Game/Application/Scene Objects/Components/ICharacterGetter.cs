using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface ICharacterGetter : IExposableComponent
    {
        CharacterFromDatabaseParameters GetCharacter();
    }
}