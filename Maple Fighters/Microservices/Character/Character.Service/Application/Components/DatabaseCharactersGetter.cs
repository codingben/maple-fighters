using System.Collections.Generic;
using System.Linq;
using CharacterService.Application.Components.Interfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using Game.Common;
using ServiceStack.OrmLite;

namespace CharacterService.Application.Components
{
    internal class DatabaseCharactersGetter : Component, IDatabaseCharactersGetter
    {
        private const int MAXIMUM_CHARACTERS = 3;
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public IEnumerable<CharacterParameters> GetCharacters(int userId)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                // Should only be 3 characters.
                var charactersFromDatabase = db.Select<CharactersTableDefinition>().Where(x => x.UserId == userId);
                var charactersDatabase = charactersFromDatabase.Select(Utils.GetCharacterParameters).ToList();

                // If there is no characters, it means that there is an empty character. 
                var length = MAXIMUM_CHARACTERS - charactersDatabase.Count;
                for (var i = 0; i < length; i++)
                {
                    charactersDatabase.Add(new CharacterParameters { HasCharacter = false, Index = CharacterIndex.Zero });
                }

                // Make an order of characters which will be sent to a client.
                var characters = new List<CharacterParameters>(MAXIMUM_CHARACTERS)
                {
                    new CharacterParameters { HasCharacter = false, Index = CharacterIndex.First },
                    new CharacterParameters { HasCharacter = false, Index = CharacterIndex.Second },
                    new CharacterParameters { HasCharacter = false, Index = CharacterIndex.Third }
                };

                foreach (var character in charactersDatabase)
                {
                    if (character.HasCharacter)
                    {
                        characters[(int)character.Index] = new CharacterParameters(character.Name, character.CharacterType, character.Index, character.LastMap);
                    }
                }
                return characters;
            }
        }
    }
}