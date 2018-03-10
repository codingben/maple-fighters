using ComponentModel.Common;
using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterGetter : IExposableComponent
    {
        CharacterParameters? GetCharacter(int userId, int characterIndex);
    }
}