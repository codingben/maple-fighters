using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseUserIdViaAccessTokenProvider : Component<IServerEntity>, IDatabaseUserIdViaAccessTokenProvider
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
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