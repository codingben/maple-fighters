using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterExistence
    {
        bool Exists(int userId, CharacterIndex characterIndex);
    }
}