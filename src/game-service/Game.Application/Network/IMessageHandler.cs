namespace Game.Network
{
    public interface IMessageHandler<T>
        where T : class
    {
        void Handle(T message);
    }
}