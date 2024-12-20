using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Services.CharacterProviderApi
{
    using Random = UnityEngine.Random;

    public class DummyCharacterProviderApi : MonoBehaviour, ICharacterProviderApi
    {
        public static DummyCharacterProviderApi GetInstance()
        {
            if (instance == null)
            {
                var characterProviderApi =
                    new GameObject("Dummy CharacterProvider Api");
                instance =
                    characterProviderApi.AddComponent<DummyCharacterProviderApi>();
            }

            return instance;
        }

        private static DummyCharacterProviderApi instance;

        public Action<long, string> CreateCharacterCallback { get; set; }

        public Action<long, string> DeleteCharacterCallback { get; set; }

        public Action<long, string> GetCharactersCallback { get; set; }

        private Dictionary<int, CharacterData> characters;
        private const string key = "characters";

        private void Awake()
        {
            characters = new Dictionary<int, CharacterData>();
        }

        private void OnDestroy()
        {
            ApiProvider.RemoveCharacterProviderApi();
        }

        public void CreateCharacter(
            string userid,
            string charactername,
            int index,
            int classindex)
        {
            var statusCode = (long)StatusCodes.Created;
            var json = string.Empty;

            var id = GenerateId();
            var characterCollection = GetCharacterCollection();

            foreach (var character in characterCollection)
            {
                if (character.userid == userid &&
                    character.charactername == charactername)
                {
                    statusCode = (long)StatusCodes.BadRequest;
                    json = "Please choose a different character name.";

                    CreateCharacterCallback?.Invoke(statusCode, json);
                    return;
                }
            }

            characters.Add(id, new CharacterData()
            {
                id = id,
                userid = userid,
                characterlevel = 1,
                characterexperience = 0f,
                charactername = charactername,
                index = index,
                classindex = classindex
            });

            SaveCharacterCollection();

            CreateCharacterCallback?.Invoke(statusCode, json);
        }

        public void UpdateCharacter(int characterid, int characterlevel, float characterexperience)
        {
            if (characters.Count == 0)
            {
                var userMetadata = FindObjectOfType<UserMetadata>();
                var userId = userMetadata?.UserData.id ?? string.Empty;

                GetCharacters(userId);
            }

            if (characters.TryGetValue(characterid, out CharacterData characterData))
            {
                characterData.characterlevel = characterlevel;
                characterData.characterexperience = characterexperience;
                characters[characterid] = characterData;
            }

            SaveCharacterCollection();
        }

        public void DeleteCharacter(int characterid)
        {
            var statusCode = (long)StatusCodes.Ok;
            var json = string.Empty;

            if (characters.Remove(characterid))
            {
                SaveCharacterCollection();

                DeleteCharacterCallback?.Invoke(statusCode, json);
            }
            else
            {
                statusCode = (long)StatusCodes.BadRequest;
                json = "The character was not found.";

                DeleteCharacterCallback?.Invoke(statusCode, json);
            }
        }

        public void GetCharacters(string userid)
        {
            var statusCode = (long)StatusCodes.Ok;
            var json = PlayerPrefs.GetString(key);

            var characterDataCollection = JsonUtility.FromJson<CharacterDataCollection>(json);
            if (characterDataCollection != null)
            {
                var characterItems = characterDataCollection.items;

                foreach (var character in characterItems)
                {
                    if (characters.ContainsKey(character.id))
                    {
                        continue;
                    }

                    characters.Add(character.id, new CharacterData()
                    {
                        id = character.id,
                        userid = character.userid,
                        charactername = character.charactername,
                        characterlevel = character.characterlevel,
                        characterexperience = character.characterexperience,
                        index = character.index,
                        classindex = character.classindex
                    });
                }
            }

            GetCharactersCallback?.Invoke(statusCode, json);
        }

        private void SaveCharacterCollection()
        {
            var characterCollection = GetCharacterCollection();
            var characterDataCollection = new CharacterDataCollection(characterCollection);

            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.SetString(key, characterDataCollection.ToString());
        }

        private CharacterData[] GetCharacterCollection()
        {
            return characters.Values.ToArray();
        }

        private int GenerateId()
        {
            return Random.Range(1, 10000);
        }
    }
}