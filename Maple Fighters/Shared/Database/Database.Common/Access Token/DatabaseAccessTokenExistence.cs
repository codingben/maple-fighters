using CommonTools.Log;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseAccessTokenExistence : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Exists(string accessToken)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var exists = db.Exists<AccessTokensTableDefinition>(new { AccessToken = accessToken });
                return exists;
            }
        }
    }
}