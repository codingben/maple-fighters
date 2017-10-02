using System;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Database.Common.TablesDefinition
{
    [Alias("users")]
    public class UsersTableDefinition
    {
        [Alias("id"), AutoIncrement]
        public int Id { get; set; }

        [Alias("email")]
        public string Email { get; set; }

        [Alias("password")]
        public string Password { get; set; }

        [Alias("first_name")]
        public string FirstName { get; set; }

        [Alias("last_name")]
        public string LastName { get; set; }

        [Alias("date_creation"), Default(OrmLiteVariables.SystemUtc)]
        public DateTime CreationDateTime { get; set; }
    }
}