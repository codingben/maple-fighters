using System;
using Proyecto26;
using ScriptableObjects.Configurations;
using UnityEngine;

namespace Scripts.Services.CharacterProviderApi
{
    public class HttpCharacterProviderApi : MonoBehaviour, ICharacterProviderApi
    {
        public static HttpCharacterProviderApi GetInstance()
        {
            if (instance == null)
            {
                var characterProviderApi =
                    new GameObject("Http CharacterProvider Api");
                instance =
                    characterProviderApi.AddComponent<HttpCharacterProviderApi>();
            }

            return instance;
        }

        private static HttpCharacterProviderApi instance;

        public Action<long, string> CreateCharacterCallback { get; set; }

        public Action<long, string> DeleteCharacterCallback { get; set; }

        public Action<long, string> GetCharactersCallback { get; set; }

        private string url;

        private void Awake()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                var uriBuilder = new UriBuilder()
                {
                    Scheme = "http",
                    Host = networkConfiguration.GetHost(),
                    Path = "character"
                };
                url = uriBuilder.Uri.ToString();
            }
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
            var characterData = new NewCharacterData()
            {
                userid = userid,
                charactername = charactername,
                index = index,
                classindex = classindex
            };

            RestClient.Post($"{url}/characters", characterData, OnCreateCharacterCallback);
        }

        private void OnCreateCharacterCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            CreateCharacterCallback?.Invoke(statusCode, json);
        }

        public void UpdateCharacter(int characterid, int characterlevel, float characterexperience)
        {
            // Left blank intentionally
        }

        public void DeleteCharacter(int characterid)
        {
            RestClient.Delete($"{url}/characters/{characterid}", OnDeleteCharacterCallback);
        }

        private void OnDeleteCharacterCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            DeleteCharacterCallback?.Invoke(statusCode, json);
        }

        public void GetCharacters(string userid)
        {
            RestClient.Get($"{url}/characters/{userid}", OnGetCharactersCallback);
        }

        private void OnGetCharactersCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            GetCharactersCallback?.Invoke(statusCode, json);
        }
    }
}