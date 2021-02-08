namespace Game.Messages
{
    public struct BubbleNotificationMessage
    {
        public int NotifierId { get; set; }

        public string Message { get; set; }

        public int Time { get; set; }
    }
}