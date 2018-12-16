using System.Collections;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class MapSceneTitleText : UserInterfaceBaseFadeEffect
    {
        public string Text
        {
            set
            {
                var textMeshPro = GetComponent<TextMeshProUGUI>();
                textMeshPro.text = value;
            }
        }

        [Header("Timer")]
        [SerializeField] private float time;

        private void Start()
        {
            Show(() => StartCoroutine(HideAfterSomeTime()));
        }

        private IEnumerator HideAfterSomeTime()
        {
            yield return new WaitForSeconds(time);
            Hide(() => UserInterfaceContainer.GetInstance().Remove(this));
        }
    }
}