using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.PlayerData
{
    [RequireComponent(typeof(UICanvasGroup), typeof(Slider))]
    public class PlayerExperiencePointsBar : UIElement, IPlayerExperiencePointsView
    {
        [SerializeField]
        private Slider experiencePointsBar;

        [SerializeField]
        private Text experiencePointsText;

        public void AddExperiencePoints(float value)
        {
            if (experiencePointsBar != null)
            {
                experiencePointsBar.value += value;
            }

            UpdateExperiencePercentage();
        }

        public void SetExperiencePoints(float value)
        {
            if (experiencePointsBar != null)
            {
                experiencePointsBar.value = value;
            }

            UpdateExperiencePercentage();
        }

        public void SetMaxExperiencePoints(float value)
        {
            if (experiencePointsBar != null)
            {
                experiencePointsBar.maxValue = value;
            }

            SetZeroExperiencePercentage();
        }

        private float GetExperiencePoints()
        {
            if (experiencePointsBar == null)
            {
                return 0;
            }

            return experiencePointsBar.value;
        }

        private float GetMaxExperiencePoints()
        {
            if (experiencePointsBar == null)
            {
                return 0;
            }

            return experiencePointsBar.maxValue;
        }

        private void SetZeroExperiencePercentage()
        {
            if (experiencePointsText != null)
            {
                experiencePointsText.text = "0" + "%" + " " + "EXP";
            }
        }

        private void UpdateExperiencePercentage()
        {
            if (experiencePointsText != null)
            {
                var value = GetExperiencePoints();
                var maxValue = GetMaxExperiencePoints();
                var percentage = CalculateExperiencePercentage(value, maxValue);

                experiencePointsText.text = percentage + "%" + " " + "EXP";
            }
        }

        private float CalculateExperiencePercentage(float value, float maxValue)
        {
            return (float)Math.Round(value / maxValue * 100f, 2);
        }
    }
}