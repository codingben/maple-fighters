using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface IPresenceScene : IExposableComponent
    {
        IScene Scene { get; set; }
    }
}