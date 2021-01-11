namespace Scripts.UI.CharacterSelection
{
    public struct UICharacterDetails
    {
        private int characterId;
        private string characterName;
        private UICharacterIndex uiCharacterIndex;
        private UICharacterClass uiCharacterClass;

        public UICharacterDetails(
            int characterId,
            string characterName,
            UICharacterIndex uiCharacterIndex,
            UICharacterClass uiCharacterClass)
        {
            this.characterId = characterId;
            this.characterName = characterName;
            this.uiCharacterIndex = uiCharacterIndex;
            this.uiCharacterClass = uiCharacterClass;
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

        public int GetCharacterId()
        {
            return characterId;
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
    }
}