using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class CharacterData : ComponentBase, ICharacterData
    {
        private string name;
        private byte characterType;

        public CharacterData()
        {
            // Left blank intentionally
        }

        public void Set(string name, byte characterType)
        {
            this.name = name;
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