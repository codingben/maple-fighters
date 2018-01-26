using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface IDatabaseCharacterExistence : IExposableComponent
    {
        bool Exists(int userId, CharacterIndex characterIndex);
    }
}