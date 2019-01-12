using TMPro;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class GameServerSelectorRefreshImage : UIElement
    {
        public string Message
        {
            set
            {
                if (messageText != null)
                {
                    messageText.text = value;
                }
            }
        }

        [Header("Text"), SerializeField]
        private TextMeshProUGUI messageText;
    }
}