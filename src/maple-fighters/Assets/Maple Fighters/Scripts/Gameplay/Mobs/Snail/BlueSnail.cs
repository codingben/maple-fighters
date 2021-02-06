using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Player;
using Scripts.Gameplay.Player.Behaviours;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Mobs
{
    public class BlueSnail : MonoBehaviour
    {
        [Header("Attack"), SerializeField]
        private Vector2 hitAmount;

        private IGameApi gameApi;

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.Attacked += OnPlayerAttacked;
        }

        private void OnDisable()
        {
            gameApi.Attacked -= OnPlayerAttacked;
        }

        private void OnPlayerAttacked(AttackedMessage message)
        {
            var entity =
                EntityContainer.GetInstance().GetLocalEntity();
            var spawnedCharacter =
                entity?.GameObject.GetComponent<ISpawnedCharacter>();
            var character =
                spawnedCharacter?.GetCharacterGameObject();
            if (character != null)
            {
                var normalized =
                    (character.transform.position - transform.position).normalized;
                var direction = new Vector2(
                    x: (normalized.x > 0 ? 1 : -1) * hitAmount.x,
                    y: hitAmount.y);

                var attackPlayer = character.GetComponent<IAttackPlayer>();
                attackPlayer?.OnPlayerAttacked(direction);
            }
        }
    }
}