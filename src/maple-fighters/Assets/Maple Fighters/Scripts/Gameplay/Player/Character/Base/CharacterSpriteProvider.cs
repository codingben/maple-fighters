using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterSpriteProvider : MonoBehaviour, ICharacterSpriteGameObject
    {
        [SerializeField]
        private GameObject characterSprite;

        public GameObject Provide()
        {
            if (characterSprite == null)
            {
                Debug.LogError("The character sprite is null.");
            }

            return characterSprite;
        }
    }
}