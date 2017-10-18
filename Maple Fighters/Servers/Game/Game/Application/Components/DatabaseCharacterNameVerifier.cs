using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServiceStack.OrmLite;

namespace Game.Application.Components
{
    internal class DatabaseCharacterNameVerifier : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Verify(string name)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var isVerified = db.Exists<CharactersTableDefinition>(new { Name = name });
                return isVerified;
            }
        }
    }
}