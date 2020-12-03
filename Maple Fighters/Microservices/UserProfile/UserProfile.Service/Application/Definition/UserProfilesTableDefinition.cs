using ServiceStack.DataAnnotations;

namespace Database.Common.TablesDefinition
{
    [Alias("profiles")]
    public class UserProfilesTableDefinition
    {
        [Alias("user_id")]
        public int UserId { get; set; }

        [Alias("is_connected")]
        public byte IsConnected { get; set; }

        [Alias("current_server")]
        public byte CurrentServer { get; set; }

        [Alias("local_id")]
        public int LocalId { get; set; }
    }
}