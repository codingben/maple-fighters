namespace Game.Application.Objects.Components
{
    public interface IMessageSender
    {
        void SendMessage(byte[] rawData, int id);
    }
}