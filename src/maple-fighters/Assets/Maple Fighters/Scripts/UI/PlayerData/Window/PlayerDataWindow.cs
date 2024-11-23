using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.PlayerData
{
    [RequireComponent(typeof(UICanvasGroup))]
    public class PlayerDataWindow : UIElement, IPlayerDataView
    {
        [SerializeField]
        private Text playerLevelText;

        [SerializeField]
        private Text playerNameText;

        [SerializeField]
        private Slider healthPointsBar;

        [SerializeField]
        private Text healthPointsText;

        public void SetLevel(int level)
        {
            if (playerLevelText != null)
            {
                playerLevelText.text = "Lv." + " " + level;
            }
        }

        public void SetName(string name)
        {
            if (playerNameText != null)
            {
                playerNameText.text = name;
            }
        }

        public void SetHealthPoints(int value)
        {
            if (healthPointsBar != null)
            {
                healthPointsBar.value = value <= 0 ? 0 : value;
            }

            if (healthPointsText != null)
            {
                healthPointsText.text = (value <= 0 ? 0 : value) + " " + "HP";
            }
        }

        public void SetMaxHealthPoints(int value)
        {
            if (healthPointsBar != null)
            {
                healthPointsBar.maxValue = value;
            }

            if (healthPointsText != null)
            {
                healthPointsText.text = value + " " + "HP";
            }
        }
    }
}