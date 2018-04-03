using CharacterService.Application.Components.Interfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace CharacterService.Application.Components
{
    internal class DatabaseCharacterRemover : Component, IDatabaseCharacterRemover
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Remove(int userId, int characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                db.Delete<CharactersTableDefinition>(c => c.UserId == userId && c.CharacterIndex == characterIndex);
            }
        }
    }
}