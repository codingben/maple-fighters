using ComponentModel.Common;
using Shared.Game.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterCreator : IExposableComponent
    {
        void Create(int userId, string name, CharacterClasses characterClass, CharacterIndex characterIndex);
    }
}