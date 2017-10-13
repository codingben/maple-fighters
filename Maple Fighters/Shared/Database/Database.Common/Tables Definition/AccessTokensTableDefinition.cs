using ServiceStack.DataAnnotations;

namespace Database.Common.TablesDefinition
{
    [Alias("access_tokens")]
    public class AccessTokensTableDefinition
    {
        [Alias("user_id")]
        public int UserId { get; set; }

        [Alias("access_token")]
        public string AccessToken { get; set; }
    }
}