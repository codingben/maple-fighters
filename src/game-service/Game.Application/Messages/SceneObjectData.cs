namespace Game.Application
{
    public class SceneObjectData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public byte Direction { get; set; }

        public CharacterData Character { get; set; }

        public bool HasCharacter { get; set; }
    }
}