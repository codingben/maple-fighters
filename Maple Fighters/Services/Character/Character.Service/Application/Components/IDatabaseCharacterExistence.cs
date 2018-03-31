using ComponentModel.Common;
using Game.Common;

namespace CharacterService.Application.Components
{
    internal interface IDatabaseCharacterExistence : IExposableComponent
    {
        bool Exists(int userId, CharacterIndex characterIndex);
    }
}