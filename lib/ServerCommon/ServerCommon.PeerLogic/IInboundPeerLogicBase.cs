using Common.ComponentModel;

namespace ServerCommon.PeerLogic
{
    /// <summary>
    /// Exposes a safe access to the inbound peer logic.
    /// </summary>
    public interface IInboundPeerLogicBase
    {
        IExposedComponents ExposedComponents { get; }
    }
}