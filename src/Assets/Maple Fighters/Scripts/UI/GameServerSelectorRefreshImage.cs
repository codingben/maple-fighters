using Scripts.UI.Core;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Windows
{
    public class GameServerSelectorRefreshImage : UserInterfaceBaseFadeEffect
    {
        public string Message
        {
            set
            {
                MessageText.text = value;
            }
        }

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI MessageText;

        protected override void OnAwake()
        {
            base.OnAwake();

            IsShowed = true;
        }
    }
}