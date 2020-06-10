namespace Game.Application
{
    public class AnimationStateChangedMessage
    {
        public int GameObjectId { get; set; }

        public byte AnimationState { get; set; }
    }
}