using ComponentModel.Common;

namespace Game.InterestManagement
{
    public class PresenceScene : Component<ISceneObject>, IPresenceScene
    {
        public IScene Scene { get; set; }

        public PresenceScene(IScene scene = null)
        {
            Scene = scene;
        }
    }
}