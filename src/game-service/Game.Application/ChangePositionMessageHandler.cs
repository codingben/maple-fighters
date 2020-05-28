namespace Game.Application
{
    public class ChangePositionMessageHandler : IMessageHandler
    {
        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.GetMessage<ChangePositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;
        }
    }
}