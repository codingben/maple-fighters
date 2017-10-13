using CommonTools.Log;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseUserIdViaAccessTokenProvider : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public int GetUserId(string accessToken)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var characterTable = db.Single<AccessTokensTableDefinition>(x => x.AccessToken == accessToken);
                return characterTable.UserId;
            }
        }
    }
}