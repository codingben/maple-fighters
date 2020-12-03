using CharacterService.Application.Components.Interfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;

namespace CharacterService.Application.Components
{
    internal class DatabaseCharacterMapUpdater : Component, IDatabaseCharacterMapUpdater
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Update(int userId, byte map)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                db.UpdateOnly(() => new CharactersTableDefinition
                {
                    LastMap = map
                }, @where: p => p.UserId == userId);
            }
        }
    }
}