using UI.Manager;
using UnityEngine;

namespace Scripts.UI.MapScene
{
    public class MapSceneTitleCreator : MonoBehaviour
    {
        private const string DefaultTitleText = "Map Name";

        [SerializeField]
        private string titleText;

        private void Awake()
        {
            if (string.IsNullOrEmpty(titleText))
            {
                titleText = DefaultTitleText;
            }

            IMapSceneTitleView mapSceneTitleView = UIElementsCreator
                .GetInstance().Create<MapSceneTitleText>();
            mapSceneTitleView.Text = titleText;

            Destroy(gameObject);
        }
    }
}