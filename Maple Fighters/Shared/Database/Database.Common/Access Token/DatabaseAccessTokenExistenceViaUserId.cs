using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseAccessTokenExistenceViaUserId : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Exists(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var exists = db.Exists<AccessTokensTableDefinition>(new AccessTokensTableDefinition { UserId = userId });
                return exists;
            }
        }
    }
}