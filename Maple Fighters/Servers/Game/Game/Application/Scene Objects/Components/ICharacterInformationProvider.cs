using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface ICharacterInformationProvider : IExposableComponent
    {
        string GetCharacterName();
        CharacterClasses GetCharacterClass();
    }
}