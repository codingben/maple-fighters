using Game.Common;

namespace Scripts.UI.Controllers
{
    public struct UiCharacterDetails
    {
        private string characterName;
        private CharacterIndex characterIndex;
        private CharacterClasses characterClass;

        public void SetCharacterName(string characterName)
        {
            this.characterName = characterName;
        }

        public void SetCharacterIndex(CharacterIndex characterIndex)
        {
            this.characterIndex = characterIndex;
        }

        public void SetCharacterClass(CharacterClasses characterClass)
        {
            this.characterClass = characterClass;
        }

        public string GetCharacterName()
        {
            return characterName;
        }

        public CharacterIndex GetCharacterIndex()
        {
            return characterIndex;
        }

        private CharacterClasses GetCharacterClass()
        {
            return characterClass;
        }
    }
}