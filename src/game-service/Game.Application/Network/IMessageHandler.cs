namespace Game.Network
{
    public interface IMessageHandler
    {
        void Handle(byte[] rawData);
    }
}