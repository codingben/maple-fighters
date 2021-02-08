namespace Game.Messages
{
    public class EnteredSceneMessage
    {
        public int GameObjectId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Direction { get; set; }

        public string CharacterName { get; set; }

        public byte CharacterClass { get; set; }
    }
}