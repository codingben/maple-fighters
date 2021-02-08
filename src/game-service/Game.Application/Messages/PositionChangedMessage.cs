namespace Game.Messages
{
    public struct PositionChangedMessage
    {
        public int GameObjectId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }
    }
}