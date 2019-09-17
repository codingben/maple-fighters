namespace Scripts.UI.Models
{
    public struct CharacterDetails
    {
        private string characterName;
        private UICharacterIndex uiCharacterIndex;
        private UICharacterClass uiCharacterClass;
        private UIMapIndex uiMapIndex;
        private bool hasCharacter;

        public CharacterDetails(
            string characterName,
            UICharacterIndex uiCharacterIndex,
            UICharacterClass uiCharacterClass,
            UIMapIndex uiMapIndex,
            bool hasCharacter)
        {
            this.characterName = characterName;
            this.uiCharacterIndex = uiCharacterIndex;
            this.uiCharacterClass = uiCharacterClass;
            this.uiMapIndex = uiMapIndex;
            this.hasCharacter = hasCharacter;
        }

        public void SetCharacterName(string characterName)
        {
            this.characterName = characterName;
        }

        public void SetCharacterIndex(UICharacterIndex uiCharacterIndex)
        {
            this.uiCharacterIndex = uiCharacterIndex;
        }

        public void SetCharacterClass(UICharacterClass uiCharacterClass)
        {
            this.uiCharacterClass = uiCharacterClass;
        }

        public void SetHasCharacter(bool hasCharacter)
        {
            this.hasCharacter = hasCharacter;
        }

        public void SetMapIndex(UIMapIndex uiMapIndex)
        {
            this.uiMapIndex = uiMapIndex;
        }

        public string GetCharacterName()
        {
            return characterName;
        }

        public UICharacterIndex GetCharacterIndex()
        {
            return uiCharacterIndex;
        }

        public UICharacterClass GetCharacterClass()
        {
            return uiCharacterClass;
        }

        public UIMapIndex GetMapIndex()
        {
            return uiMapIndex;
        }

        public bool HasCharacter()
        {
            return hasCharacter;
        }
    }
}