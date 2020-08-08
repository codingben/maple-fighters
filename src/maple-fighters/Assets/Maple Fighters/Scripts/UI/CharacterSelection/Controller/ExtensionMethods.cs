using Scripts.Gameplay.Player;

namespace Scripts.UI.CharacterSelection
{
    public static class ExtensionMethods
    {
        public static UICharacterIndex ToUiCharacterIndex(this byte index)
        {
            var uiCharacterIndex = UICharacterIndex.Zero;

            switch (index)
            {
                case 0:
                {
                    uiCharacterIndex = UICharacterIndex.Zero;
                    break;
                }

                case 1:
                {
                    uiCharacterIndex = UICharacterIndex.First;
                    break;
                }

                case 2:
                {
                    uiCharacterIndex = UICharacterIndex.Second;
                    break;
                }

                case 3:
                {
                    uiCharacterIndex = UICharacterIndex.Third;
                    break;
                }
            }

            return uiCharacterIndex;
        }

        public static byte FromUiCharacterIndex(
            this UICharacterIndex uiCharacterIndex)
        {
            var index = default(byte);

            switch (uiCharacterIndex)
            {
                case UICharacterIndex.Zero:
                {
                    index = 0;
                    break;
                }

                case UICharacterIndex.First:
                {
                    index = 1;
                    break;
                }

                case UICharacterIndex.Second:
                {
                    index = 2;
                    break;
                }

                case UICharacterIndex.Third:
                {
                    index = 3;
                    break;
                }
            }

            return index;
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
    }
}