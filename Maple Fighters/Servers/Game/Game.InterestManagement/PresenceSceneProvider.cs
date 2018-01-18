using ComponentModel.Common;

namespace Game.InterestManagement
{
    public class PresenceSceneProvider : Component<ISceneObject>, IPresenceSceneProvider
    {
        public IScene Scene { get; set; }

        public PresenceSceneProvider(IScene scene = null)
        {
            Scene = scene;
        }
    }
}