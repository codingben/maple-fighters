using ComponentModel.Common;
using Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface ICharacterGetter : IExposableComponent
    {
        CharacterParameters GetCharacter();
    }
}