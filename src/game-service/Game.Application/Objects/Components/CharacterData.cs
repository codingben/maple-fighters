using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    public class CharacterData : ComponentBase, ICharacterData
    {
        private string name;
        private byte characterType;

        public CharacterData()
        {
            // Left blank intentionally
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetCharacterType(byte characterType)
        {
            this.characterType = characterType;
        }

        public string GetName()
        {
            return name;
        }

        public byte GetCharacterType()
        {
            return characterType;
        }
    }
}