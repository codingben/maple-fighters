using System;
using Authorization.Server.Common;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;
using PeerLogic.Common.Components;

namespace Authorization.Client.Common
{
    public class UnauthorizedClientPeerLogic<TNewPeerLogic> : PeerLogicBase<AuthorizationOperations, EmptyEventCode>
        where TNewPeerLogic : class, IPeerLogicBase
    {
        protected override void OnInitialized()
        {
            AddCommonComponents();
            AddComponents();

            AddHandlerForAuthorizationOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout(seconds: 15, lookForOperations: false));
        }

        private void AddHandlerForAuthorizationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(AuthorizationOperations.Authorize, new AuthorizationOperationHandler(OnAuthorized, OnNonAuthorized));
        }

        private void OnAuthorized(int userId)
        {
            var newPeerLogic = Activator.CreateInstance(typeof(TNewPeerLogic), userId) as TNewPeerLogic;
            ClientPeerWrapper.SetPeerLogic(newPeerLogic);
        }

        private void OnNonAuthorized()
        {
            var peerId = ClientPeerWrapper.PeerId;
            var ip = ClientPeerWrapper.Peer.ConnectionInformation.Ip;
            var port = ClientPeerWrapper.Peer.ConnectionInformation.Port;

            LogUtils.Log(MessageBuilder.Trace($"An authorization for peer {ip}:{port} with id {peerId} has been failed."));

            ClientPeerWrapper.Peer.Disconnect();
        }
    }
}