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
            var mapSceneTitleText = UIElementsCreator.GetInstance()
                .Create<MapSceneTitleText>();
            mapSceneTitleText.Text = titleText;

            Destroy(gameObject);
        }
    }
}