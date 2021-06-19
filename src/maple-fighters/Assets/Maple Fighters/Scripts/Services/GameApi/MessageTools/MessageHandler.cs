namespace Game.MessageTools
{
    public class MessageHandler<T> : IMessageHandler<T>
        where T : class
    {
        private readonly UnityEvent<T> handler;

        public MessageHandler(UnityEvent<T> handler)
        {
            this.handler = handler;
        }

        public void Handle(T message)
        {
            handler?.Invoke(message);
        }
    }
}