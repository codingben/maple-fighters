namespace Game.Application
{
    public class GameObjectData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public CharacterData Character { get; set; }

        public bool HasCharacter { get; set; }
    }
}