namespace Scripts.UI
{
    public struct UiCharacterDetails
    {
        private string characterName;
        private UiCharacterIndex uiCharacterIndex;
        private UiCharacterClass uiCharacterClass;
        private UiMapIndex uiMapIndex;
        private bool hasCharacter;

        public void SetCharacterName(string characterName)
        {
            this.characterName = characterName;
        }

        public void SetCharacterIndex(UiCharacterIndex uiCharacterIndex)
        {
            this.uiCharacterIndex = uiCharacterIndex;
        }

        public void SetCharacterClass(UiCharacterClass uiCharacterClass)
        {
            this.uiCharacterClass = uiCharacterClass;
        }

        public void SetHasCharacter(bool hasCharacter)
        {
            this.hasCharacter = hasCharacter;
        }

        public string GetCharacterName()
        {
            return characterName;
        }

        public UiCharacterIndex GetCharacterIndex()
        {
            return uiCharacterIndex;
        }

        public UiCharacterClass GetCharacterClass()
        {
            return uiCharacterClass;
        }

        public bool HasCharacter()
        {
            return hasCharacter;
        }
    }
}