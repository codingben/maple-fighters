using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class MenuBackgroundController : MonoBehaviour
    {
        private void Awake()
        {
            UIElementsCreator.GetInstance()
                .Create<MenuBackgroundImage>(UILayer.Background);
        }
    }
}