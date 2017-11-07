using MathematicsHelper;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.SceneObjects
{
    public class Portal : SceneObject
    {
        private const string SCENE_OBJECT_NAME = "Portal";
        private const int DIRECTION_LEFT = 1;

        public Portal(Vector2 position, int map) 
            : base(SCENE_OBJECT_NAME, position, DIRECTION_LEFT)
        {
            Container.AddComponent(new PortalInfoProvider(map));
        }
    }
}