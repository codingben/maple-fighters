using System;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace Database.Common.AccessToken
{
    public class DatabaseAccessTokenCreator : Component, IDatabaseAccessTokenCreator
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
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

        private static string GenerateAccessToken => Guid.NewGuid().ToString("N");
    }
}