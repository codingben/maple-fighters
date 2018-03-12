using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace Registration.Application.Components
{
    internal class DatabaseUserEmailVerifier : Component, IDatabaseUserEmailVerifier
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Verify(string email)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var isVerified = db.Exists<UsersTableDefinition>(new { Email = email });
                return isVerified;
            }
        }
    }
}