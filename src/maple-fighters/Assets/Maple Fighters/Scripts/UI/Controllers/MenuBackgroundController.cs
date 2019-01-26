using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class MenuBackgroundController : MonoBehaviour
    {
        private MenuBackgroundImage menuBackgroundImage;

        private void Awake()
        {
            menuBackgroundImage = 
                UIElementsCreator.GetInstance()
                    .Create<MenuBackgroundImage>(UILayer.Background);
        }

        private void OnDestroy()
        {
            if (menuBackgroundImage != null)
            {
                Destroy(menuBackgroundImage.gameObject);
            }
        }
    }
}