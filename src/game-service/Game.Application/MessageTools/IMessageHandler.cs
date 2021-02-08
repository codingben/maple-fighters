namespace Game.MessageTools
{
    public interface IMessageHandler<T>
        where T : struct
    {
        void Handle(T message);
    }
}