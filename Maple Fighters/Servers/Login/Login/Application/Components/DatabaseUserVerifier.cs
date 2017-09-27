using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Login.Application.Components
{
    internal class DatabaseUserVerifier : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>();
        }

        public bool IsExists(string email)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var isExists = db.Exists<UsersTableDefinition>(new { Email = email });
                return isExists;
            }
        }
    }
}