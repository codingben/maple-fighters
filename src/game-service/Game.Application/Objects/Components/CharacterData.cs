using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class CharacterData : ComponentBase, ICharacterData
    {
        private string name;

        private byte characterType;

        public CharacterData(string name, byte characterType)
        {
            this.name = name;
            this.characterType = characterType;
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