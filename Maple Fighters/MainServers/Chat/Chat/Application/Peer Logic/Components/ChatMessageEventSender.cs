using Chat.Application.PeerLogic.Components.Interfaces;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common;
using PeerLogic.Common.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;

namespace Chat.Application.PeerLogic.Components
{
    internal class ChatMessageEventSender : Component, IChatMessageEventSender
    {
        private IPeerContainer peerContainer;
        private IClientPeerProvider clientPeerProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = ServerComponents.GetComponent<IPeerContainer>().AssertNotNull();
            clientPeerProvider = Components.GetComponent<IClientPeerProvider>().AssertNotNull();
        }

        public void SendChatMessage(string message)
        {
            foreach (var peerWrapper in peerContainer.GetAllPeerWrappers())
            {
                if (peerWrapper.PeerId == clientPeerProvider.PeerId)
                {
                    continue;
                }

                RaiseChatMessage(peerWrapper);
            }

            void RaiseChatMessage(IClientPeerWrapper peerWrapper)
            {
                var eventSenderWrapper = peerWrapper.PeerLogic.Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
                var parameters = new ChatMessageEventParameters(message);
                eventSenderWrapper.Send((byte)ChatEvents.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
            }
        }
    }
}