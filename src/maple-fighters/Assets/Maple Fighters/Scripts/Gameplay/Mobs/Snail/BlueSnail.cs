using Game.Common;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Player;
using Scripts.Gameplay.Player.Behaviours;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Mobs
{
    public class BlueSnail : MonoBehaviour
    {
        [Header("Attack"), SerializeField]
        private Vector2 hitAmount;

        private GameService gameService;

        private void Start()
        {
            gameService = FindObjectOfType<GameService>();
            gameService?.GameSceneApi?.PlayerAttacked.AddListener(OnPlayerAttacked);
        }

        private void OnDisable()
        {
            gameService?.GameSceneApi?.PlayerAttacked.RemoveListener(OnPlayerAttacked);
        }

        private void OnPlayerAttacked(PlayerAttackedEventParameters parameters)
        {
            var entity = EntityContainer.GetInstance().GetLocalEntity()
                ?.GameObject;
            var spawnedCharacter = entity?.GetComponent<ISpawnedCharacter>();
            var character = spawnedCharacter?.GetCharacterGameObject();
            if (character != null)
            {
                var point = 
                    new Vector3(parameters.ContactPointX, parameters.ContactPointY);
                var direction = new Vector2(
                    x: ((character.transform.position - point).normalized.x
                        > 0
                            ? 1
                            : -1) * hitAmount.x,
                    y: hitAmount.y);

                var attackPlayer = character.GetComponent<IAttackPlayer>();
                attackPlayer?.OnPlayerAttacked(direction);
            }
        }
    }
}