using Scripts.Editor;
using Scripts.Gameplay.Graphics;
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
        }

        private void OnUnFadeEffectStarted()
        {
            if (health <= 0)
            {
                deadAnimation?.Play();
                attackedAnimation?.Stop();
            }
        }
    }
}