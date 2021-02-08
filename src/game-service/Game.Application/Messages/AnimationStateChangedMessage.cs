namespace Game.Messages
{
    public struct AnimationStateChangedMessage
    {
        public int GameObjectId { get; set; }

        public byte AnimationState { get; set; }
    }
}