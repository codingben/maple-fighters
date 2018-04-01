using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterCreator
    {
        void Create(int userId, string name, CharacterClasses characterClass, CharacterIndex characterIndex);
    }
}