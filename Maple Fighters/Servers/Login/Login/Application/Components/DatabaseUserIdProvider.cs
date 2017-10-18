using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Login.Application.Components
{
    internal class DatabaseUserIdProvider : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public int GetUserId(string email)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = db.Single<UsersTableDefinition>(x => x.Email == email);
                return user.Id;
            }
        }
    }
}