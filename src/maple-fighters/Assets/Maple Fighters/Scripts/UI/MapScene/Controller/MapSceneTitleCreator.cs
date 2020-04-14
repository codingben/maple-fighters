using UI.Manager;
using UnityEngine;

namespace Scripts.UI.MapScene
{
    public class MapSceneTitleCreator : MonoBehaviour
    {
        [SerializeField]
        private string messageText;

        [SerializeField]
        private float seconds = 2;

        private void Awake()
        {
            IMessageView messageView =
                UIElementsCreator.GetInstance().Create<MessageText>();
            messageView.Text = messageText;
            messageView.Seconds = seconds;

            Destroy(gameObject);
        }
    }
}