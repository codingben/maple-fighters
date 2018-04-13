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
            var outboundServerPeerLogicBase = new OutboundServerPeerLogicBase<TOperationCode, TEventCode>(outboundServerPeer);
            outboundServerPeerLogicBase.Initialize();
            return outboundServerPeerLogicBase;
        }

        public static IOutboundServerPeerLogic CreateCommonServerAuthenticationPeerLogic(this IOutboundServerPeer outboundServerPeer, 
            string secretKey, Action onAuthenticated)
        {
            var commonServerAuthenticationPeerLogic = new CommonServerAuthenticationPeerLogic(outboundServerPeer, 
                secretKey, onAuthenticated);
            commonServerAuthenticationPeerLogic.Initialize();
            return commonServerAuthenticationPeerLogic;
        }
    }
}