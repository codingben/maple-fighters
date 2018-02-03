using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseUserIdViaAccessTokenProvider : Component, IDatabaseUserIdViaAccessTokenProvider
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
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