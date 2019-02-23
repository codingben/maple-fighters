using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class MenuBackgroundController : MonoBehaviour
    {
        private MenuBackgroundImage backgroundImage;

        private void Awake()
        {
            CreateMenuBackgroundImage();
        }

        private void CreateMenuBackgroundImage()
        {
            backgroundImage = UIElementsCreator.GetInstance()
                .Create<MenuBackgroundImage>(UILayer.Background);
        }

        private void OnDestroy()
        {
            DestroyMenuBackgroundImage();
        }

        private void DestroyMenuBackgroundImage()
        {
            if (backgroundImage != null)
            {
                Destroy(backgroundImage.gameObject);
            }
        }
    }
}