using ComponentModel.Common;
using InterestManagement.Components.Interfaces;

namespace InterestManagement.Components
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