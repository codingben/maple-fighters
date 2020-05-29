namespace Game.Application
{
    public class ChangePlayerPositionMessage
    {
        public float X { get; set; }

        public float Y { get; set; }

        public byte Direction { get; set; }
    }
}