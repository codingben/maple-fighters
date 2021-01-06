namespace Scripts.Gameplay.Player
{
    public class CharacterData
    {
        public string Name { get; set; }

        public byte Class { get; set; }

        public float Direction { get; set; }

        public CharacterData(string name, byte @class, float direction)
        {
            Name = name;
            Class = @class;
            Direction = direction;
        }
    }
}