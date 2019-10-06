namespace Scripts.UI.CharacterSelection
{
    public static class Utils
    {
        private static string GetCharacterPath(CharacterDetails characterDetails)
        {
            var characterIndex = characterDetails.GetCharacterIndex();
            var characterClass = characterDetails.GetCharacterClass();
            var hasCharacter = characterDetails.HasCharacter();
            var name =
                hasCharacter
                    ? $"{characterClass} {(int)characterIndex}"
                    : $"Sample {(int)characterIndex}";

            return $"Characters/{name}";
        }
    }
}