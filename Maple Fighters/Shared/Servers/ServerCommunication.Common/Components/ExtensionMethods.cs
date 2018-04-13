using System;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    public static class ExtensionMethods
    {
        public static IOutboundServerPeerLogic CreateOutboundServerPeerLogic<TOperationCode, TEventCode>(this IOutboundServerPeer outboundServerPeer)
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible
        {
            IOutboundServerPeerLogic outboundServerPeerLogicBase = new OutboundServerPeerLogicBase<TOperationCode, TEventCode>(outboundServerPeer);
            outboundServerPeerLogicBase.Initialize();
            return outboundServerPeerLogicBase;
        }

        public static IOutboundServerPeerLogicBase CreateCommonServerAuthenticationPeerLogic(this IOutboundServerPeer outboundServerPeer, 
            string secretKey, Action onAuthenticated)
        {
            IOutboundServerPeerLogicBase commonServerAuthenticationPeerLogic = new CommonServerAuthenticationPeerLogic(outboundServerPeer, 
                secretKey, onAuthenticated);
            commonServerAuthenticationPeerLogic.Initialize();
            return commonServerAuthenticationPeerLogic;
        }
    }
}