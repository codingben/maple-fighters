namespace Game.Application
{
    public class ChangePositionMessageHandler : IMessageHandler
    {
        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.GetMessage<ChangePlayerPositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;
        }
    }
}