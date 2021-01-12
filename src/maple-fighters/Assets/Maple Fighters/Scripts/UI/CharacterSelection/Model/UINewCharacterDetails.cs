namespace Scripts.UI.CharacterSelection
{
    public struct UINewCharacterDetails
    {
        private string characterName;
        private UICharacterClass uiCharacterClass;

        public UINewCharacterDetails(string characterName, UICharacterClass uiCharacterClass)
        {
            this.characterName = characterName;
            this.uiCharacterClass = uiCharacterClass;
        }

        public void SetCharacterName(string characterName)
        {
            this.characterName = characterName;
        }

        public void SetCharacterClass(UICharacterClass uiCharacterClass)
        {
            this.uiCharacterClass = uiCharacterClass;
        }

        public string GetCharacterName()
        {
            return characterName;
        }

        public UICharacterClass GetCharacterClass()
        {
            return uiCharacterClass;
        }
    }
}