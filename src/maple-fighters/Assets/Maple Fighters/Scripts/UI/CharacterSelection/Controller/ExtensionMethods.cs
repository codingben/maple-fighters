using Game.Common;

namespace Scripts.UI.CharacterSelection
{
    public static class ExtensionMethods
    {
        public static UICharacterIndex ToUiCharacterIndex(
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

        public static CharacterIndex FromUiCharacterIndex(
            this UICharacterIndex uiCharacterIndex)
        {
            var characterIndex = CharacterIndex.Zero;

            switch (uiCharacterIndex)
            {
                case UICharacterIndex.Zero:
                {
                    characterIndex = CharacterIndex.Zero;
                    break;
                }

                case UICharacterIndex.First:
                {
                    characterIndex = CharacterIndex.First;
                    break;
                }

                case UICharacterIndex.Second:
                {
                    characterIndex = CharacterIndex.Second;
                    break;
                }

                case UICharacterIndex.Third:
                {
                    characterIndex = CharacterIndex.Third;
                    break;
                }
            }

            return characterIndex;
        }

        public static UICharacterClass ToUiCharacterClass(
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

        public static CharacterClasses FromUiCharacterClass(
            this UICharacterClass uiCharacterClass)
        {
            var characterClass = CharacterClasses.Knight;

            switch (uiCharacterClass)
            {
                case UICharacterClass.Knight:
                {
                    characterClass = CharacterClasses.Knight;
                    break;
                }

                case UICharacterClass.Arrow:
                {
                    characterClass = CharacterClasses.Arrow;
                    break;
                }

                case UICharacterClass.Wizard:
                {
                    characterClass = CharacterClasses.Wizard;
                    break;
                }
            }

            return characterClass;
        }

        public static UIMapIndex ToUiMapIndex(this Maps map)
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