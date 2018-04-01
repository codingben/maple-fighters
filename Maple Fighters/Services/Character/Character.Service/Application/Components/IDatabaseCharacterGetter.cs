using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterGetter
    {
        CharacterParameters? GetCharacter(int userId, int characterIndex);
    }
}