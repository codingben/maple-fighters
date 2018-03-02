using Game.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay.Actors
{
    public class CharacterName : MonoBehaviour
    {
        [SerializeField] private Text nameText;

        public void OnDirectionChanged(Directions directions)
        {
            const float SCALE = 0.01f;

            switch (directions)
            {
                case Directions.Left:
                {
                    nameText.transform.localScale = new Vector3(SCALE, SCALE, nameText.transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    nameText.transform.localScale = new Vector3(-SCALE, SCALE, nameText.transform.localScale.z);
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