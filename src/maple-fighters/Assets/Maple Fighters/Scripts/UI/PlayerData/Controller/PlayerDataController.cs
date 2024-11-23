using UI;
using UnityEngine;
using Scripts.Services;

namespace Scripts.UI.PlayerData
{
    public class PlayerDataController : MonoBehaviour
    {
        private IPlayerDataView playerDataView;
        private IPlayerExperiencePointsView playerExperiencePointsView;

        private UserMetadata userMetadata;

        private void Awake()
        {
            userMetadata = FindObjectOfType<UserMetadata>();
            userMetadata.CharacterExperiencePointsAdded += OnCharacterExperiencePointsAdded;
            userMetadata.CharacterLevelUp += OnCharacterLevelUp;
        }

        private void Start()
        {
            CreatePlayerDataWindow();
            CreatePlayerExperiencePointsBar();
        }

        private void OnDestroy()
        {
            userMetadata.CharacterExperiencePointsAdded -= OnCharacterExperiencePointsAdded;
            userMetadata.CharacterLevelUp -= OnCharacterLevelUp;
        }

        public void SetPlayerHealth(int value)
        {
            playerDataView?.SetHealthPoints(value);
        }

        public void SetMaxPlayerHealth(int value)
        {
            playerDataView?.SetMaxHealthPoints(value);
        }

        private void CreatePlayerDataWindow()
        {
            playerDataView = UICreator
                .GetInstance()
                .Create<PlayerDataWindow>();
            playerDataView.SetLevel(userMetadata.CharacterLevel);
            playerDataView.SetName(userMetadata.CharacterName);
        }

        private void CreatePlayerExperiencePointsBar()
        {
            playerExperiencePointsView = UICreator
                .GetInstance()
                .Create<PlayerExperiencePointsBar>();
            playerExperiencePointsView.SetMaxExperiencePoints(userMetadata.GetMaxExperiencePoints());
            playerExperiencePointsView.SetExperiencePoints(userMetadata.GetExperiencePoints());
        }

        private void CreateLevelUpEffect()
        {
            var levelUpText = UICreator
                .GetInstance()
                .Create<LevelUpText>();
            levelUpText.Hide();
        }

        private void OnCharacterExperiencePointsAdded(float value)
        {
            playerExperiencePointsView?.AddExperiencePoints(value);
        }

        private void OnCharacterLevelUp(int value)
        {
            playerDataView?.SetLevel(userMetadata.CharacterLevel);

            playerExperiencePointsView?.SetMaxExperiencePoints(userMetadata.GetMaxExperiencePoints());
            playerExperiencePointsView?.SetExperiencePoints(userMetadata.GetExperiencePoints());

            CreateLevelUpEffect();
        }
    }
}