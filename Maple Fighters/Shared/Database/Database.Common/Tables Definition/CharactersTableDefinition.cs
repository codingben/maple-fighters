using ServiceStack.DataAnnotations;

namespace Database.Common.TablesDefinition
{
    [Alias("characters")]
    public class CharactersTableDefinition
    {
        [Alias("user_id")]
        public int UserId { get; set; }

        [Alias("name")]
        public string Name { get; set; }

        [Alias("character_type")]
        public string CharacterType { get; set; }

        [Alias("character_index")]
        public int CharacterIndex { get; set; }
    }
}