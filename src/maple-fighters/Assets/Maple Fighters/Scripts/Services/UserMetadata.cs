using UnityEngine;
using Scripts.Services.AuthenticatorApi;
using System.Text.RegularExpressions;
using System;

namespace Scripts.Services
{
    public class UserMetadata : MonoBehaviour
    {
        public event Action<float> CharacterExperiencePointsAdded;

        public event Action<int> CharacterLevelUp;

        public UserData UserData { get; set; }

        public int CharacterType { get; set; }

        public int CharacterHealth { get; set; }

        public int CharacterLevel { get; set; }

        public string CharacterName { get; set; }

        public float CharacterExperiencePoints { get; set; }

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

            CharacterHealth = GetMaxCharacterHealth();
            CharacterLevel = 1;
            CharacterExperiencePoints = 0;
        }

        public void AddExperiencePoints(float value)
        {
            CharacterExperiencePoints += value;

            VerifyCharacterLevel(value);
        }

        public int GetMaxCharacterHealth()
        {
            return 500;
        }

        public float GetExperiencePoints()
        {
            return CharacterExperiencePoints;
        }

        public float GetMaxExperiencePoints()
        {
            return CharacterLevel * 100;
        }

        private void VerifyCharacterLevel(float value)
        {
            if (CharacterExperiencePoints >= GetMaxExperiencePoints())
            {
                CharacterExperiencePoints = 0;
                CharacterLevel++;

                CharacterLevelUp?.Invoke(CharacterLevel);
            }
            else
            {
                CharacterExperiencePointsAdded?.Invoke(value);
            }
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