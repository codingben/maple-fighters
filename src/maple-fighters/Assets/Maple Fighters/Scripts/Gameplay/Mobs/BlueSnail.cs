using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Mobs
{
    public class BlueSnail : MonoBehaviour
    {
        [Header("Attack"), SerializeField]
        private Vector2 hitAmount;

        private void Start()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.PlayerAttacked.AddListener(OnPlayerAttacked);
            }
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.PlayerAttacked.RemoveListener(OnPlayerAttacked);
            }
        }

        private void OnPlayerAttacked(PlayerAttackedEventParameters parameters)
        {
            var player =
                SceneObjectsContainer.GetInstance().GetLocalSceneObject()
                    .GameObject;
            if (player != null)
            {
                var spawnedCharacter = player.GetComponent<ISpawnedCharacter>();
                var character = spawnedCharacter.GetCharacterGameObject();
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
                    attackPlayer.OnPlayerAttacked(direction);
                }
            }
        }
    }
}