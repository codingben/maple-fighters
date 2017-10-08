using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class DatabaseCharactersGetter : Component<IServerEntity>
    {
        private const int MAXIMUM_CHARACTERS = 3;
        private DatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.Container.GetComponent<DatabaseConnectionProvider>();
        }

        public IEnumerable<Character?> GetCharacters(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var query = db.From<CharactersTableDefinition>().Where(x => x.UserId == userId);
                var results = db.Column<CharactersTableDefinition>(query);

                var characters = new List<Character?>(MAXIMUM_CHARACTERS);
                characters.AddRange(results.Select(GetCharacter));

                var orderedCharacters = characters.OrderBy(x => x != null ? x.Value.Index : 0).ToList();
                return orderedCharacters;
            }
        }

        public Character? GetCharacter(int userId, int characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var characterTable = db.Single<CharactersTableDefinition>(x => x.UserId == userId && x.CharacterIndex == characterIndex);
                return GetCharacter(characterTable);
            }
        }

        private Character? GetCharacter(CharactersTableDefinition charactersTableDefinition)
        {
            if (charactersTableDefinition.CharacterType == CharacterClasses.Arrow.ToString())
            {
                return new Character(CharacterClasses.Arrow, charactersTableDefinition.Name, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Knight.ToString())
            {
                return new Character(CharacterClasses.Knight, charactersTableDefinition.Name, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Wizard.ToString())
            {
                return new Character(CharacterClasses.Wizard, charactersTableDefinition.Name, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            LogUtils.Log(MessageBuilder.Trace($"Can not get character type of user id #{charactersTableDefinition.UserId}"));
            return null;
        }
    }
}