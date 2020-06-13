using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    public class MessageSender : ComponentBase, IMessageSender
    {
        public void SendMessage(byte[] rawData, int id)
        {
            throw new System.NotImplementedException();
        }
    }
}