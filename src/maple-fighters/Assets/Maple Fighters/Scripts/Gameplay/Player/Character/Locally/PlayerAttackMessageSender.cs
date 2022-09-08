using Game.Messages;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PlayerAttackMessageSender : MonoBehaviour
    {
        private IGameApi gameApi;
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
        }

        private void OnCharacterSpawned()
        {
            var playerController = spawnedCharacter
                .GetCharacter()
                .GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.PlayerStateChanged += OnPlayerStateChanged;
            }
        }

        private void OnPlayerStateChanged(PlayerStates playerState)
        {
            if (playerState == PlayerStates.PrimaryAttack)
            {
                gameApi?.SendMessage(MessageCodes.AttackMob, new AttackMobMessage());
            }
        }
    }
}