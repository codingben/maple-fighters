using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.Components
{
    internal class DatabaseUserProfileCreator : Component, IDatabaseUserProfileCreator
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Create(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = new UserProfilesTableDefinition
                {
                    UserId = userId,
                    CurrentServer = (byte)serverType,
                    IsConnected = (byte)connectionStatus,
                    LocalId = localId
                };
                db.Insert(user);
            }
        }
    }
}