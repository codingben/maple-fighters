using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay.Actors
{
    public class CharacterName : MonoBehaviour
    {
        [SerializeField] private Text nameText;

        public void SetName(string name)
        {
            nameText.text = name;
        }

        public void SetSortingOrder(int index)
        {
            nameText.GetComponent<Canvas>().sortingOrder = index;
        }
    }
}