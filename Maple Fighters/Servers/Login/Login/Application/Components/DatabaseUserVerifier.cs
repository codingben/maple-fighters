using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace Login.Application.Components
{
    internal class DatabaseUserVerifier : Component, IDatabaseUserVerifier
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public bool IsExists(string email)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var isExists = db.Exists<UsersTableDefinition>(new { Email = email });
                return isExists;
            }
        }
    }
}