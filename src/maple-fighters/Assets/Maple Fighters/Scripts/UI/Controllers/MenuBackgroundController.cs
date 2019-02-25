using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class MenuBackgroundController : MonoBehaviour
    {
        private IMenuBackgroundView backgroundView;

        private void Awake()
        {
            CreateMenuBackgroundImage();
        }

        private void CreateMenuBackgroundImage()
        {
            backgroundView = UIElementsCreator.GetInstance()
                .Create<MenuBackgroundImage>(UILayer.Background);
        }

        private void OnDestroy()
        {
            DestroyMenuBackgroundImage();
        }

        private void DestroyMenuBackgroundImage()
        {
            // TODO: Implement
        }
    }
}