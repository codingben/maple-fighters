using Common.ComponentModel;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    public class BubbleNotificationMessageSender : ComponentBase, IBubbleNotifier
    {
        private IMessageSender messageSender;

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();
        }

        public void Notify(int notifierId, string text, int time)
        {
            var messageCode = (byte)MessageCodes.BubbleNotification;
            var message = new BubbleNotificationMessage()
            {
                NotifierId = notifierId,
                Message = text,
                Time = time
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}