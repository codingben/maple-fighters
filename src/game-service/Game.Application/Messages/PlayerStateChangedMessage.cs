namespace Game.Application
{
    public class PlayerStateChangedMessage
    {
        public byte PlayerState { get; set; }

        public int SceneObjectId { get; set; }
    }
}