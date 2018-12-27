using CommonTools.Log;
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
            var canvas = nameText.GetComponent<Canvas>().AssertNotNull();
            canvas.sortingOrder = index;
        }
    }
}