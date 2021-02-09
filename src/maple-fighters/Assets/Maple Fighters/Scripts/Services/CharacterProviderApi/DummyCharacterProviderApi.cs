using System;
using System.Collections.Generic;
using System.Linq;
using Proyecto26;
using UnityEngine;

namespace Scripts.Services.CharacterProviderApi
{
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

        private List<CharacterData> characters;
        private int id;

        private void Awake()
        {
            characters = new List<CharacterData>();
        }

        public void CreateCharacter(
            int userid,
            string charactername,
            int index,
            int classindex)
        {
            var statusCode = 0;
            var json = string.Empty;

            // On server it will iterate only by user (id) characters
            foreach (var characterData in characters)
            {
                if (characterData.userid == userid
                    && characterData.charactername == charactername)
                {
                    statusCode = 400;
                    json = "Please choose a different character name.";

                    CreateCharacterCallback?.Invoke(statusCode, json);
                    return;
                }
            }

            characters.Add(new CharacterData()
            {
                id = id++,
                userid = userid,
                charactername = charactername,
                index = index,
                classindex = classindex
            });

            statusCode = 201;

            CreateCharacterCallback?.Invoke(statusCode, json);
        }

        public void DeleteCharacter(int characterid)
        {
            var item = default(CharacterData);
            var itemExists = false;

            foreach (var characterData in characters)
            {
                if (characterData.id == characterid)
                {
                    item = characterData;
                    itemExists = true;
                }
            }

            var statusCode = 0;
            var json = string.Empty;

            if (itemExists)
            {
                characters.Remove(item);

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

        public void GetCharacters(int userid)
        {
            var items = characters.Where(x => x.userid == userid).ToArray();
            var statusCode = 200;
            var json = "[]";

            if (items.Length != 0)
            {
                json = JsonHelper.ArrayToJsonString(items);
            }

            GetCharactersCallback?.Invoke(statusCode, json);
        }
    }
}