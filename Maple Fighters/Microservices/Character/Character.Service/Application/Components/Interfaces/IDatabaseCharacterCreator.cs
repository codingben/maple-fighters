using Game.Common;

namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharacterCreator
    {
        void Create(int userId, string name, CharacterClasses characterClass, CharacterIndex characterIndex);
    }
}