namespace Scripts.UI
{
    public struct UICharacterDetails
    {
        private string characterName;
        private UICharacterIndex uiCharacterIndex;
        private UICharacterClass uiCharacterClass;
        private UIMapIndex uiMapIndex;
        private bool hasCharacter;

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

        public bool HasCharacter()
        {
            return hasCharacter;
        }
    }
}
