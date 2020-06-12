namespace Game.Application.Network
{
    public interface IMessageHandler
    {
        void Handle(byte[] rawData);
    }
}