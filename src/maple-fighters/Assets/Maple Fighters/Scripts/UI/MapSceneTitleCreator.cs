using UI.Manager;
using UnityEngine;

namespace Scripts.UI
{
    public class MapSceneTitleCreator : MonoBehaviour
    {
        [SerializeField]
        private string titleText;

        private void Awake()
        {
            if (string.IsNullOrEmpty(titleText))
            {
                titleText = "Map Name";
            }

            var mapSceneTitleText = UIElementsCreator.GetInstance()
                .Create<MapSceneTitleText>();
            mapSceneTitleText.Text = titleText;


            Destroy(gameObject);
        }
    }
}