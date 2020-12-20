namespace Game.Messages
{
    public class EnterSceneMessage
    {
        public byte Map { get; set; }

        public string CharacterName { get; set; }

        public byte CharacterType { get; set; }
    }
}