using Game.Common;

namespace Scripts.UI.Controllers
{
    public static class ExtensionMethods
    {
        public static UICharacterIndex ConvertToUiCharacterIndex(
            this CharacterIndex characterIndex)
        {
            var uiCharacterIndex = UICharacterIndex.Zero;

            switch (characterIndex)
            {
                case CharacterIndex.Zero:
                {
                    uiCharacterIndex = UICharacterIndex.Zero;
                    break;
                }

                case CharacterIndex.First:
                {
                    uiCharacterIndex = UICharacterIndex.First;
                    break;
                }

                case CharacterIndex.Second:
                {
                    uiCharacterIndex = UICharacterIndex.Second;
                    break;
                }

                case CharacterIndex.Third:
                {
                    uiCharacterIndex = UICharacterIndex.Third;
                    break;
                }
            }

            return uiCharacterIndex;
        }

        public static UICharacterClass ConvertToUiCharacterClass(
            this CharacterClasses characterClasses)
        {
            var uiCharacterClass = UICharacterClass.Knight;

            switch (characterClasses)
            {
                case CharacterClasses.Knight:
                {
                    uiCharacterClass = UICharacterClass.Knight;
                    break;
                }

                case CharacterClasses.Arrow:
                {
                    uiCharacterClass = UICharacterClass.Arrow;
                    break;
                }

                case CharacterClasses.Wizard:
                {
                    uiCharacterClass = UICharacterClass.Wizard;
                    break;
                }
            }

            return uiCharacterClass;
        }

        public static UIMapIndex ConvertToUiMapIndex(this Maps map)
        {
            var uiMapIndex = UIMapIndex.Map_1;

            switch (map)
            {
                case Maps.Map_1:
                {
                    uiMapIndex = UIMapIndex.Map_1;
                    break;
                }

                case Maps.Map_2:
                {
                    uiMapIndex = UIMapIndex.Map_2;
                    break;
                }
            }

            return uiMapIndex;
        }
    }
}