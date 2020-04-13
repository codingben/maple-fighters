using UI.Manager;
using UnityEngine;

namespace Scripts.UI.MapScene
{
    public class MapSceneTitleCreator : MonoBehaviour
    {
        [SerializeField]
        private string messageText;

        [SerializeField]
        private int time = 1;

        private void Awake()
        {
            IMessageView messageView =
                UIElementsCreator.GetInstance().Create<MessageText>();
            messageView.Text = messageText;

            Destroy(gameObject);
        }
    }
}