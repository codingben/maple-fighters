using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

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
                var exists = db.Exists<UserProfilesTableDefinition>(new UserProfilesTableDefinition { UserId = userId });
                return exists;
            }
        }
    }
}