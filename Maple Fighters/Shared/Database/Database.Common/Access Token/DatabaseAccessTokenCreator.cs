using System;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseAccessTokenCreator : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public string Create(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var accessToken = GenerateAccessToken;
                var accessTokens = new AccessTokensTableDefinition
                {
                    UserId = userId,
                    AccessToken = accessToken
                };
                db.Insert(accessTokens);
                return accessToken;
            }
        }

        public static string GenerateAccessToken => Guid.NewGuid().ToString("N");
    }
}