using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.ServerApplication.Common.PeerLogic;

namespace Chat.Application.PeerLogic.Components
{
    internal class ChatMessageEventSender : Component<IPeerEntity>
    {
        private PeerContainer peerContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();
        }

        public void SendChatMessage(string message)
        {
            foreach (var peerWrapper in peerContainer.GetAllPeerWrappers())
            {
                if (peerWrapper.PeerId == Entity.Id)
                {
                    continue;
                }

                var eventSenderWrapper = peerWrapper.PeerLogic.Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
                var parameters = new ChatMessageEventParameters(message);
                eventSenderWrapper.Send((byte)ChatEvents.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
            }
        }
    }
}