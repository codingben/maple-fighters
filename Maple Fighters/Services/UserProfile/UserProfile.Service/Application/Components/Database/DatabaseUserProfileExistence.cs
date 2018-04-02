using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.Components
{
    internal class DatabaseUserProfileExistence : Component, IDatabaseUserProfileExistence
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Exists(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var exists = db.Exists<UserProfilesTableDefinition>(new { UserId = userId });
                return exists;
            }
        }
    }
}