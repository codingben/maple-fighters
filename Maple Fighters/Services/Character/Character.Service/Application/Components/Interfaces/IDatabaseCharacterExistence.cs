using Game.Common;

namespace CharacterService.Application.Components.Interfaces
{
    internal interface IDatabaseCharacterExistence
    {
        bool Exists(int userId, CharacterIndex characterIndex);
    }
}