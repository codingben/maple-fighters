namespace Game.Application.Messages
{
    public class GameObjectData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public string CharacterName { get; set; }

        public byte CharacterType { get; set; }

        public bool HasCharacter { get; set; }
    }
}