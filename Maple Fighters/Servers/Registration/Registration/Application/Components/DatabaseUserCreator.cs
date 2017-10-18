using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Registration.Application.Components
{
    internal class DatabaseUserCreator : Component<IServerEntity>, IDatabaseUserCreator
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Create(string email, string password, string firstName, string lastName)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = new UsersTableDefinition
                {
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                };
                db.Insert(user);
            }
        }
    }
}