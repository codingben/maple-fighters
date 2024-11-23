using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Services;
using Scripts.UI.ScreenFade;
using Scripts.UI.PlayerData;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerHealth : MonoBehaviour
    {
        private PlayerController playerController;
        private ScreenFadeController screenFadeController;
        private PlayerDataController playerDataController;
        private UserMetadata userMetadata;

        private bool isDead;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            screenFadeController = FindObjectOfType<ScreenFadeController>();
            playerDataController = FindObjectOfType<PlayerDataController>();
            userMetadata = FindObjectOfType<UserMetadata>();
        }

        private void Start()
        {
            playerDataController.SetMaxPlayerHealth(userMetadata.GetMaxCharacterHealth());
            playerDataController.SetPlayerHealth(userMetadata.CharacterHealth);
        }

        public void Damage()
        {
            if (isDead) return;

            userMetadata.CharacterHealth -= Random.Range(50, 150);

            playerDataController.SetPlayerHealth(userMetadata.CharacterHealth);

            VerifyDeath();
        }

        private void VerifyDeath()
        {
            if (userMetadata.CharacterHealth > 0) return;

            isDead = true;

            StartCoroutine(Dead());

            userMetadata.CharacterHealth = userMetadata.GetMaxCharacterHealth();
        }

        private IEnumerator Dead()
        {
            playerController.SetPlayerState(PlayerStates.Dead);

            yield return new WaitForSeconds(5f);

            LoadLobby();
        }

        private void LoadLobby()
        {
            if (screenFadeController != null)
            {
                screenFadeController.Show();
                screenFadeController.FadeInCompleted += OnFadeInCompleted;
            }
        }

        private void OnFadeInCompleted()
        {
            if (screenFadeController != null)
            {
                screenFadeController.FadeInCompleted -= OnFadeInCompleted;
            }

            SceneManager.LoadScene(sceneName: Constants.SceneNames.Maps.Lobby);
        }
    }
}