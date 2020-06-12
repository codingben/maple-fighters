namespace Game.Application.Messages
{
    public class BubbleNotificationMessage
    {
        public int RequesterId { get; set; }

        public string Message { get; set; }

        public int Time { get; set; }
    }
}