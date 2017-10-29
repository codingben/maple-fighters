using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterSpawnPositionProvider : IExposableComponent
    {
        void AddPosition(Maps map, Vector2 position);

        Vector2 GetPosition(Maps map);
    }
}