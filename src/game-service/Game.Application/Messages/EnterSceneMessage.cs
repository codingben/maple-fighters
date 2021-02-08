namespace Game.Messages
{
    public struct EnterSceneMessage
    {
        public byte Map { get; set; }

        public string CharacterName { get; set; }

        public byte CharacterType { get; set; }
    }
}