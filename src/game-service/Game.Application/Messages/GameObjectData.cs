namespace Game.Messages
{
    public struct GameObjectData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Direction { get; set; }

        public string CharacterName { get; set; }

        public byte CharacterClass { get; set; }

        public bool HasCharacter { get; set; }
    }
}