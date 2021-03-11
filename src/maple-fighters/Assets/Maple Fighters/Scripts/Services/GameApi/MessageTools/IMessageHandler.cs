namespace Game.MessageTools
{
    public interface IMessageHandler<T>
        where T : class
    {
        void Handle(T message);
    }
}