using ComponentModel.Common;

namespace Game.InterestManagement
{
    public class OrientationProvider : Component<ISceneObject>, IOrientationProvider
    {
        public Direction Direction { get; set; }

        public OrientationProvider(Direction direction)
        {
            Direction = direction;
        }
    }
}