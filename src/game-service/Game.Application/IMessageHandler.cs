namespace Game.Application
{
    public interface IMessageHandler
    {
        void Handle(byte[] rawData);
    }
}