using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components;
using Database.Common.TablesDefinition;
using ServiceStack.OrmLite;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class DatabaseCharactersGetter : Component, IDatabaseCharactersGetter
    {
        private const int MAXIMUM_CHARACTERS = 3;
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Entity.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public IEnumerable<CharacterFromDatabaseParameters> GetCharacters(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                // Should only be 3 characters.
                var charactersFromDatabase = db.Select<CharactersTableDefinition>().Where(x => x.UserId == userId);
                var charactersDatabase = charactersFromDatabase.Select(GetCharacter).ToList();

                // If there is no characters, it means that there is an empty character. 
                var length = MAXIMUM_CHARACTERS - charactersDatabase.Count;
                for (var i = 0; i < length; i++)
                {
                    charactersDatabase.Add(new CharacterFromDatabaseParameters { HasCharacter = false, Index = CharacterIndex.Zero });
                }

                // Make an order of characters which will be sent to a client.
                var characters = new List<CharacterFromDatabaseParameters>(MAXIMUM_CHARACTERS)
                {
                    new CharacterFromDatabaseParameters { HasCharacter = false, Index = CharacterIndex.First },
                    new CharacterFromDatabaseParameters { HasCharacter = false, Index = CharacterIndex.Second },
                    new CharacterFromDatabaseParameters { HasCharacter = false, Index = CharacterIndex.Third }
                };

                foreach (var character in charactersDatabase)
                {
                    if (character.HasCharacter)
                    {
                        characters[(int)character.Index] = new CharacterFromDatabaseParameters(character.Name, character.CharacterType, character.Index);
                    }
                }
                return characters;
            }
        }

        public CharacterFromDatabaseParameters? GetCharacter(int userId, int characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var characterTable = db.Single<CharactersTableDefinition>(x => x.UserId == userId && x.CharacterIndex == characterIndex);
                return GetCharacter(characterTable);
            }
        }

        private CharacterFromDatabaseParameters GetCharacter(CharactersTableDefinition charactersTableDefinition)
        {
            if (charactersTableDefinition.CharacterType == CharacterClasses.Arrow.ToString())
            {
                return new CharacterFromDatabaseParameters(charactersTableDefinition.Name, CharacterClasses.Arrow, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Knight.ToString())
            {
                return new CharacterFromDatabaseParameters(charactersTableDefinition.Name, CharacterClasses.Knight, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            if (charactersTableDefinition.CharacterType == CharacterClasses.Wizard.ToString())
            {
                return new CharacterFromDatabaseParameters(charactersTableDefinition.Name, CharacterClasses.Wizard, (CharacterIndex)charactersTableDefinition.CharacterIndex);
            }

            LogUtils.Log(MessageBuilder.Trace($"Can not get character type of user id #{charactersTableDefinition.UserId}"));
            return new CharacterFromDatabaseParameters { HasCharacter = false, Index = (CharacterIndex)charactersTableDefinition.CharacterIndex };
        }
    }
}