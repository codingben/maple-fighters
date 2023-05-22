using UnityEngine;
using Scripts.Services.AuthenticatorApi;
using System.Text.RegularExpressions;
using System;

namespace Scripts.Services
{
    public class UserMetadata : MonoBehaviour
    {
        public UserData UserData { get; set; }

        public int CharacterType { get; set; }

        public string CharacterName { get; set; }

        public bool IsLoggedIn { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            UserData = new UserData()
            {
                id = GetUserId()
            };
        }

        private string GetUserId()
        {
            string userid;
            const string key = "userid";

            if (PlayerPrefs.HasKey(key))
            {
                userid = PlayerPrefs.GetString(key);
            }
            else
            {
                userid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                userid = Regex.Replace(userid, "[/+=]", string.Empty);

                PlayerPrefs.SetString(key, userid);
                PlayerPrefs.Save();
            }

            return userid;
        }
    }
}