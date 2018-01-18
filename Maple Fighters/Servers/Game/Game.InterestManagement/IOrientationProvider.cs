using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface IOrientationProvider : IExposableComponent
    {
        Direction Direction { get; set; }
    }
}