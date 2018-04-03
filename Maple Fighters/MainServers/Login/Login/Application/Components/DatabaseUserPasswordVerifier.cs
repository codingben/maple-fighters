using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using Login.Application.Components.Interfaces;
using ServiceStack.OrmLite;

namespace Login.Application.Components
{
    internal class DatabaseUserPasswordVerifier : Component, IDatabaseUserPasswordVerifier
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Verify(string email, string password)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = db.Single<UsersTableDefinition>(x => x.Email == email);
                var isVerified = user.Password == password;
                return isVerified;
            }
        }
    }
}