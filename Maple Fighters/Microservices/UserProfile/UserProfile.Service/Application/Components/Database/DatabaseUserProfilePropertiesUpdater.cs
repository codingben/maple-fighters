using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.Components
{
    internal class DatabaseUserProfilePropertiesUpdater : Component, IDatabaseUserProfilePropertiesUpdater
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Update(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                db.UpdateOnly(() => new UserProfilesTableDefinition
                {
                    LocalId = localId,
                    CurrentServer = (byte)serverType,
                    IsConnected = (byte)connectionStatus
                }, where: p => p.UserId == userId);
            }
        }
    }
}