using Common.ComponentModel;

namespace ServerCommon.PeerLogic
{
    public interface IPeerLogicBase
    {
        IExposedComponentsProvider ExposedComponents { get; }
    }
}