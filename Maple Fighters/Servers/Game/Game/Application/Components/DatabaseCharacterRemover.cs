using CommonTools.Log;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Game.Application.Components
{
    internal class DatabaseCharacterRemover : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Remove(int userId, int characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                db.Delete<CharactersTableDefinition>(c => c.UserId == userId && c.CharacterIndex == characterIndex);
            }

            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var exists = db.Exists<CharactersTableDefinition>(new CharactersTableDefinition { UserId = userId, CharacterIndex = (int)characterIndex });
                return exists;
            }
        }
    }
}