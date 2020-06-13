namespace Game.Application.Objects.Components
{
    public interface IMessageSender
    {
        void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class;
    }
}