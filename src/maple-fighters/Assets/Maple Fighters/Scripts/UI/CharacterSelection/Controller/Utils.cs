using Scripts.Constants;

namespace Scripts.UI.CharacterSelection
{
    public static class Utils
    {
        public static string GetCharacterPath(UICharacterDetails characterDetails)
        {
            var characterClass = characterDetails.GetCharacterClass();
            var name = $"{characterClass}";

            return string.Format(Paths.Resources.Sample.Characters, name);
        }
    }
}