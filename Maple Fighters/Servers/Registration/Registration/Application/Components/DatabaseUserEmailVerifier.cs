using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Registration.Application.Components
{
    internal class DatabaseUserEmailVerifier : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>();
        }

        public bool Verify(string email)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var isVerified = db.Exists<UsersTableDefinition>(new { Email = email });
                return isVerified;
            }
        }
    }
}