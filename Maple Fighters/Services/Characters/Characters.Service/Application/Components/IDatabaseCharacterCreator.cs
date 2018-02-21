using Characters.Client.Common;
using ComponentModel.Common;

namespace CharactersService.Application.Components
{
    internal interface IDatabaseCharacterCreator : IExposableComponent
    {
        void Create(int userId, string name, CharacterClasses characterClass, CharacterIndex characterIndex);
    }
}