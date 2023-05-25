using System;
using System.Collections.Generic;
using System.Linq;
using Proyecto26;
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
            var statusCode = 0;
            var json = string.Empty;

            var id = GenerateId();
            var characterCollection = GetCharacterCollection();

            foreach (var character in characterCollection)
            {
                if (character.userid == userid &&
                    character.charactername == charactername)
                {
                    statusCode = 400;
                    json = "Please choose a different character name.";

                    CreateCharacterCallback?.Invoke(statusCode, json);
                    return;
                }
            }

            characters.Add(id, new CharacterData()
            {
                id = id,
                userid = userid,
                charactername = charactername,
                index = index,
                classindex = classindex
            });

            SaveCharacterCollection();

            statusCode = 201;

            CreateCharacterCallback?.Invoke(statusCode, json);
        }

        public void DeleteCharacter(int characterid)
        {
            var statusCode = 0;
            var json = string.Empty;

            if (characters.Remove(characterid))
            {
                SaveCharacterCollection();

                statusCode = 200;

                DeleteCharacterCallback?.Invoke(statusCode, json);
            }
            else
            {
                statusCode = 404;
                json = "The character was not found.";

                DeleteCharacterCallback?.Invoke(statusCode, json);
            }
        }

        public void GetCharacters(string userid)
        {
            var statusCode = 200;
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