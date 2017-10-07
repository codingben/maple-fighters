using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Game.Application.Components
{
    internal class DatabaseCharacterCreator : Component<IServerEntity>
    {
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>();
        }

        public void Create(int id, string name, string characterType, int characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = new CharactersTableDefinition
                {
                    UserId = id,
                    Name = name,
                    CharacterType = characterType,
                    CharacterIndex = characterIndex
                };
                db.Insert(user);
            }
        }
    }
}