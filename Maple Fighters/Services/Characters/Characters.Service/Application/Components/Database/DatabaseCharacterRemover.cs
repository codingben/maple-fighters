using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace CharactersService.Application.Components
{
    internal class DatabaseCharacterRemover : Component, IDatabaseCharacterRemover
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public bool Remove(int userId, int characterIndex)
        {
            // TODO: SRP

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