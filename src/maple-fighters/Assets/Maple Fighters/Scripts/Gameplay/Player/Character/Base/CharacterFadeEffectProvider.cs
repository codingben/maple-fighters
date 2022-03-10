using Scripts.Gameplay.Graphics;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class CharacterFadeEffectProvider : MonoBehaviour, IFadeEffectProvider
    {
        private FadeEffect fadeEffect;
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var characterSprite = spawnedCharacter.GetCharacterSprite();
            if (characterSprite != null)
            {
                fadeEffect = characterSprite.GetComponent<FadeEffect>();
                fadeEffect?.SetOwner(gameObject);
            }
        }

        public FadeEffect Provide()
        {
            if (fadeEffect == null)
            {
                Debug.LogWarning("Character fade effect provider is null.");
            }

            return fadeEffect;
        }
    }
}