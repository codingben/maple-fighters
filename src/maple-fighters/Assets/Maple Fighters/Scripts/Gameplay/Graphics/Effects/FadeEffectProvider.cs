using Scripts.Gameplay.Graphics;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class FadeEffectProvider : MonoBehaviour, IFadeEffectProvider
    {
        [SerializeField]
        private FadeEffect fadeEffect;

        private void Awake()
        {
            fadeEffect?.SetOwner(gameObject);
        }

        public FadeEffect Provide()
        {
            if (fadeEffect == null)
            {
                Debug.LogWarning("Fade effect provider is null.");
            }

            return fadeEffect;
        }
    }
}