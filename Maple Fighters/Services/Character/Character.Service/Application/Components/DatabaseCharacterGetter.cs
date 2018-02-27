using Character.Client.Common;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace CharacterService.Application.Components
{
    internal class DatabaseCharacterGetter : Component, IDatabaseCharacterGetter
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public CharacterFromDatabaseParameters? GetCharacter(int userId, int characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var charactersTable = db.Single<CharactersTableDefinition>(x => x.UserId == userId && x.CharacterIndex == characterIndex);
                var character = Utils.GetCharacterParameters(charactersTable);
                return character;
            }
        }
    }
}