namespace Game.Messages
{
    public struct ChatMessage
    {
        public int SenderId { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string ContentFormatted { get; set; }
    }
}