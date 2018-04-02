using Game.Common;

namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharacterGetter
    {
        CharacterParameters? GetCharacter(int userId, int characterIndex);
    }
}