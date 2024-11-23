using Scripts.Gameplay.Graphics;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    public class MobBehaviour : MonoBehaviour
    {
        [SerializeField]
        private FadeEffect fadeEffect;

        [SerializeField]
        private MobAttackedAnimation attackedAnimation;

        [SerializeField]
        private MobDeadAnimation deadAnimation;

        [SerializeField]
        private int health = 100;

        [SerializeField]
        private float mobExperience = 10;

        [Header("Mob Damage Effect")]
        [SerializeField]
        private GameObject mobDamageText;

        [SerializeField]
        private Transform mobDamagePosition;

        private void Awake()
        {
            if (fadeEffect != null)
            {
                fadeEffect.UnFadeEffectStarted += OnUnFadeEffectStarted;
            }
        }

        private void OnDestroy()
        {
            if (fadeEffect != null)
            {
                fadeEffect.UnFadeEffectStarted -= OnUnFadeEffectStarted;
            }
        }

        public void Attack(int damageAmount)
        {
            health -= damageAmount;

            if (health > 0)
            {
                attackedAnimation?.Play();
            }

            CreateDamageText(damageAmount);
        }

        private void OnUnFadeEffectStarted()
        {
            if (health <= 0)
            {
                deadAnimation?.Play();
                attackedAnimation?.Stop();

                var userMetadata = FindObjectOfType<UserMetadata>();
                if (userMetadata != null)
                {
                    userMetadata.AddExperiencePoints(mobExperience);
                }
            }
        }

        private void CreateDamageText(int damageAmount)
        {
            var gameObject = Instantiate(
                mobDamageText,
                mobDamagePosition.position,
                Quaternion.identity);
            var text = gameObject.GetComponent<MobDamageText>();
            text.SetText(damageAmount >= 1 ? damageAmount.ToString() : "Miss");
        }
    }
}