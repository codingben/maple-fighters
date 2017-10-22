using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.Application.SceneObjects
{
    internal class PortalInfoProvider : Component<ISceneObject>, IPortalInfoProvider
    {
        public Vector2 PlayerPosition { get; }
        public Maps Map { get; }

        public PortalInfoProvider(Vector2 playerPosition, int map)
        {
            PlayerPosition = playerPosition;
            Map = (Maps)map;
        }
    }
}