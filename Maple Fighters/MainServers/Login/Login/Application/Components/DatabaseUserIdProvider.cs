using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using Login.Application.Components.Interfaces;
using ServiceStack.OrmLite;

namespace Login.Application.Components
{
    internal class DatabaseUserIdProvider : Component, IDatabaseUserIdProvider
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public int GetUserId(string email)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = db.Single<UsersTableDefinition>(x => x.Email == email);
                return user.Id;
            }
        }
    }
}