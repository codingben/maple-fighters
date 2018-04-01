namespace Chat.Application.PeerLogic.Components
{
    internal interface IChatMessageEventSender
    {
        void SendChatMessage(string message);
    }
}