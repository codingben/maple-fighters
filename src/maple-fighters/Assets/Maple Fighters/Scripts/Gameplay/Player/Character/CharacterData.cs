namespace Scripts.Gameplay.Player
{
    public class CharacterData
    {
        public int Id { get; set; }

        public string CharacterName { get; set; }

        public byte CharacterType { get; set; }

        public CharacterData(int id, string characterName, byte characterType)
        {
            Id = id;
            CharacterName = characterName;
            CharacterType = characterType;
        }
    }
}