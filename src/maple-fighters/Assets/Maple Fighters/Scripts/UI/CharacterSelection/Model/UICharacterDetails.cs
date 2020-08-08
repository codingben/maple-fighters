namespace Scripts.UI.CharacterSelection
{
    public struct UICharacterDetails
    {
        private string characterName;
        private UICharacterIndex uiCharacterIndex;
        private UICharacterClass uiCharacterClass;
        private string mapName;
        private bool hasCharacter;

        public UICharacterDetails(
            string characterName,
            UICharacterIndex uiCharacterIndex,
            UICharacterClass uiCharacterClass,
            string mapName,
            bool hasCharacter)
        {
            this.characterName = characterName;
            this.uiCharacterIndex = uiCharacterIndex;
            this.uiCharacterClass = uiCharacterClass;
            this.mapName = mapName;
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

        public void SetMapName(string mapName)
        {
            this.mapName = mapName;
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

        public string GetMapName()
        {
            return mapName;
        }

        public bool HasCharacter()
        {
            return hasCharacter;
        }
    }
}