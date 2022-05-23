using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(ChatController))]
    public class WelcomeMessageSender : MonoBehaviour
    {
        private const string WelcomeMessage = "<b>Welcome <color=yellow>{0}</color> to Maple Fighters! <color=red>❤</color></b>";
        private static bool isWelcomePlayerMessageSent;

        private ChatController chatController;

        private void Start()
        {
            chatController = GetComponent<ChatController>();
            chatController.CharacterNameChanged += OnCharacterNameChanged;
        }

        private void OnCharacterNameChanged(string name)
        {
            if (isWelcomePlayerMessageSent) return;

            isWelcomePlayerMessageSent = true;

            chatController.AddMessage(string.Format(WelcomeMessage, name));
        }
    }
}