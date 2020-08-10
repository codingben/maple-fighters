namespace Scripts.Gameplay.Player
{
    public class CharacterData
    {
        public string Name { get; set; }

        public byte Class { get; set; }

        public CharacterData(string name, byte @class = 0)
        {
            Name = name;
            Class = @class;
        }
    }
}