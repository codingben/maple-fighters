using Common.ComponentModel;

namespace ServerCommon.PeerLogic
{
    /// <summary>
    /// Exposes a safe access to the peer logic.
    /// </summary>
    public interface IPeerLogicBase
    {
        IExposedComponentsProvider ExposedComponents { get; }
    }
}