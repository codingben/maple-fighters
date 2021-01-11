using Scripts.Constants;

namespace Scripts.UI.CharacterSelection
{
    public static class Utils
    {
        public static string GetCharacterPath(UICharacterDetails characterDetails)
        {
            var characterClass = characterDetails.GetCharacterClass();
            var characterIndex = characterDetails.GetCharacterIndex();
            var name = $"{characterClass} {(int)characterIndex}";

            return string.Format(Paths.Resources.Sample.Characters, name);
        }
    }
}