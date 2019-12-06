using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterSpriteProvider : MonoBehaviour
    {
        [SerializeField]
        private GameObject characterSprite;

        public GameObject GetCharacterSprite()
        {
            return characterSprite;
        }
    }
}