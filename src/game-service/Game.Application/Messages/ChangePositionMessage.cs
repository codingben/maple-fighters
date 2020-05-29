namespace Game.Application
{
    public class ChangePositionMessage
    {
        public float X { get; set; }

        public float Y { get; set; }

        public byte Direction { get; set; }
    }
}