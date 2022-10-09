using Game.Messages;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map.Objects;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PlayerAttackMessageSender : MonoBehaviour
    {
        [SerializeField]
        private float hitDistance = 1.5f;

        [SerializeField]
        private int minDamageAmount = 0;

        [SerializeField]
        private int maxDamageAmount = 50;

        private IGameApi gameApi;

        private void Awake()
        {
            gameApi = ApiProvider.ProvideGameApi();
        }

        public void Attack(Vector2 direction)
        {
            var raycasts =
                Physics2D.RaycastAll(transform.position, direction, hitDistance);

            foreach (var raycast in raycasts)
            {
                var isMob =
                    raycast.transform.gameObject.CompareTag(GameTags.MobTag);
                if (isMob)
                {
                    var mob =
                        raycast.transform.gameObject;

                    Attack(mob);
                }
            }
        }

        public void Attack(GameObject target)
        {
            var damageAmount =
                Random.Range(minDamageAmount, maxDamageAmount);
            var mobEntity =
                target.transform.parent.GetComponent<IEntity>();
            var mobBehaviour =
                target.transform.parent.GetComponent<MobBehaviour>();

            mobBehaviour?.Attack(damageAmount);

            var id = mobEntity?.Id ?? -1;
            var message = new AttackMobMessage
            {
                MobId = id,
                DamageAmount = damageAmount
            };

            gameApi?.SendMessage(MessageCodes.AttackMob, message);
        }
    }
}