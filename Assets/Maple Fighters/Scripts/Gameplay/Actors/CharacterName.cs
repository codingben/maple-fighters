using Shared.Game.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay.Actors
{
    public class CharacterName : MonoBehaviour
    {
        [SerializeField] private Text nameText;

        public void OnChangedDirection(Directions directions)
        {
            switch (directions)
            {
                case Directions.Left:
                {
                    nameText.transform.localScale = new Vector3(0.01f, 0.01f, 1);
                    break;
                }
                case Directions.Right:
                {
                    nameText.transform.localScale = new Vector3(-0.01f, 0.01f, 1);
                    break;
                }
            }
        }

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