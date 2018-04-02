namespace Chat.Application.PeerLogic.Components.Interfaces
{
    internal interface IChatMessageEventSender
    {
        void SendChatMessage(string message);
    }
}