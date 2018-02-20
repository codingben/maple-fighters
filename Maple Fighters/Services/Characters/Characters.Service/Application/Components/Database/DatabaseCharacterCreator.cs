using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;
using Shared.Game.Common;

namespace CharactersService.Application.Components
{
    internal class DatabaseCharacterCreator : Component, IDatabaseCharacterCreator
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Create(int userId, string name, CharacterClasses characterClass, CharacterIndex characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = new CharactersTableDefinition
                {
                    UserId = userId,
                    Name = name,
                    CharacterType = characterClass.ToString(),
                    CharacterIndex = (int)characterIndex
                };
                db.Insert(user);
            }
        }
    }
}