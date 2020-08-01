namespace Game.Network
{
    public interface IMessageHandler<TMessage>
        where TMessage : class
    {
        void Handle(TMessage message);
    }
}