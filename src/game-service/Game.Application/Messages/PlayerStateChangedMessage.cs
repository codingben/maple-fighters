namespace Game.Application
{
    public class PlayerStateChangedMessage
    {
        public int GameObjectId { get; set; }

        public byte PlayerState { get; set; }
    }
}