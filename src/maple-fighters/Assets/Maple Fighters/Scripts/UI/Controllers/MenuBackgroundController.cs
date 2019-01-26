using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class MenuBackgroundController : MonoBehaviour
    {
        private MenuBackgroundImage menuBackgroundImage;
        private BackgroundCharacters backgroundCharacters;

        private void Awake()
        {
            menuBackgroundImage = 
                UIElementsCreator.GetInstance()
                    .Create<MenuBackgroundImage>(UILayer.Background);
            backgroundCharacters = 
                UIElementsCreator.GetInstance().Create<BackgroundCharacters>(
                    UILayer.Background,
                    UIIndex.End);
        }

        private void OnDestroy()
        {
            if (menuBackgroundImage != null)
            {
                Destroy(menuBackgroundImage.gameObject);
            }

            if (backgroundCharacters != null)
            {
                Destroy(backgroundCharacters.gameObject);
            }
        }
    }
}