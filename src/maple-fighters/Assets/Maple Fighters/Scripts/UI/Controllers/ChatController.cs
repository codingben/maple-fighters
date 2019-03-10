using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ChatController : MonoBehaviour
    {
        private IChatView chatView;

        private FocusStateController focusStateController;

        private void Awake()
        {
            // TODO: Use event bus system
            focusStateController = FindObjectOfType<FocusStateController>();

            CreateAndSubscribeToChatWindow();
        }

        private void CreateAndSubscribeToChatWindow()
        {
            chatView = UIElementsCreator.GetInstance().Create<ChatWindow>();
            chatView.FocusChanged += OnFocusChanged;
            chatView.MessageAdded += OnMessageAdded;
        }

        private void OnDestroy()
        {
            UnsubscribeFromChatWindow();
        }

        private void UnsubscribeFromChatWindow()
        {
            if (chatView != null)
            {
                chatView.FocusChanged -= OnFocusChanged;
                chatView.MessageAdded -= OnMessageAdded;
            }
        }

        public void SetCharacterName(string name)
        {
            if (chatView != null)
            {
                chatView.CharacterName = name;
            }
        }

        public void OnMessageReceived(string message)
        {
            chatView?.AddMessage(message);
        }

        private void OnMessageAdded(string message)
        {
            chatView?.AddMessage(message);

            // TODO: MessageSent
        }

        private void OnFocusChanged(bool isFocused)
        {
            if (focusStateController != null)
            {
                // TODO: Shouldn't be Game state always
                var focusState =
                    isFocused ? FocusState.Chat : FocusState.Game;

                focusStateController.ChangeFocusState(focusState);
            }
        }
    }
}