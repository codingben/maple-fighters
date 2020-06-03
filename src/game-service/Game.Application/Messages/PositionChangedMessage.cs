namespace Game.Application
{
    public class PositionChangedMessage
    {
        public int GameObjectId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public byte Direction { get; set; }
    }
}