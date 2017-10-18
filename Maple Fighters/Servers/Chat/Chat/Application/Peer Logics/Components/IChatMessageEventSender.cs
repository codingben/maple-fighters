using ComponentModel.Common;

namespace Chat.Application.PeerLogic.Components
{
    internal interface IChatMessageEventSender : IExposableComponent
    {
        void SendChatMessage(string message);
    }
}