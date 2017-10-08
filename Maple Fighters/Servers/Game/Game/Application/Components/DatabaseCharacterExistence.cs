using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class DatabaseCharacterExistence : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>();
        }

        public bool Exists(int userId, CharacterIndex characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var exists = db.Exists<CharactersTableDefinition>(new CharactersTableDefinition { UserId = userId, CharacterIndex = (int)characterIndex });
                return exists;
            }
        }
    }
}