using Scripts.Constants;

namespace Scripts.UI.CharacterSelection
{
    public static class Utils
    {
        public static string GetCharacterPath(UICharacterDetails characterDetails)
        {
            var characterIndex = characterDetails.GetCharacterIndex();
            var characterClass = characterDetails.GetCharacterClass();
            var hasCharacter = characterDetails.HasCharacter();
            var name =
                hasCharacter
                    ? $"{characterClass} {(int)characterIndex}"
                    : $"{Paths.Resources.Sample.SampleCharacter} {(int)characterIndex}";

            var path = string.Format(Paths.Resources.Sample.Characters, name);
            return path;
        }
    }
}