using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseAccessTokenProvider : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public string GetAccessToken(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var characterTable = db.Single<AccessTokensTableDefinition>(x => x.UserId == userId);
                return characterTable.AccessToken;
            }
        }
    }
}