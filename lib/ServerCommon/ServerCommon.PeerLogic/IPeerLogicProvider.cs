namespace ServerCommon.PeerLogic
{
    public interface IPeerLogicProvider
    {
        void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase;

        void RemovePeerLogic();

        IPeerLogicBase ProvidePeerLogic();
    }
}