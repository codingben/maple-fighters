using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using Game.Common;
using ServiceStack.OrmLite;

namespace CharacterService.Application.Components
{
    internal class DatabaseCharacterExistence : Component, IDatabaseCharacterExistence
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
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