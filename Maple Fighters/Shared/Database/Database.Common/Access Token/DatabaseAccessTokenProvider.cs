using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseAccessTokenProvider : Component, IDatabaseAccessTokenProvider
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
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