using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface IPresenceSceneProvider : IExposableComponent
    {
        IScene Scene { get; set; }
    }
}