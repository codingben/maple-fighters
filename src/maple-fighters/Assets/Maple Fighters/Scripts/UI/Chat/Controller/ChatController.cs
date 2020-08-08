using Scripts.UI.Focus;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(ChatInteractor))]
    public class ChatController : MonoBehaviour, IOnChatMessageReceived
    {
        private ChatInteractor chatInteractor;
        private FocusStateController focusStateController;

        private IChatView chatView;

        private void Awake()
        {
            chatInteractor = GetComponent<ChatInteractor>();
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

            chatInteractor.SendChatMessage(message);
        }

        private void OnFocusChanged(bool isFocused)
        {
            var focusState = isFocused ? UIFocusState.Chat : UIFocusState.Game;
            focusStateController?.ChangeFocusState(focusState);
        }
    }
}