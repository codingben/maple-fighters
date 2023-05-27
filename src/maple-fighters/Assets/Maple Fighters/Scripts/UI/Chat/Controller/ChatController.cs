using System;
using Scripts.UI.Focus;
using UI;
using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(ChatInteractor))]
    public class ChatController : MonoBehaviour, IOnChatMessageReceived
    {
        public Action<string> CharacterNameChanged;

        private ChatInteractor chatInteractor;
        private FocusStateController focusStateController;

        private IChatView chatView;

        private void Awake()
        {
            chatInteractor = GetComponent<ChatInteractor>();
            focusStateController = FindObjectOfType<FocusStateController>();
            focusStateController.FocusChanged += OnFocusStateChanged;

            CreateAndSubscribeToChatWindow();
        }

        private void OnFocusStateChanged(UIFocusState focusState)
        {
            if (chatView != null)
            {
                if (focusState == UIFocusState.UI)
                {
                    chatView.BlockOrUnblockTyping(block: true);
                }
                else if (chatView.IsTypingBlocked)
                {
                    chatView.BlockOrUnblockTyping(block: false);
                }
            }
        }

        private void CreateAndSubscribeToChatWindow()
        {
            chatView = UICreator
                .GetInstance()
                .Create<ChatWindow>();

            if (chatView != null)
            {
                chatView.FocusChanged += OnFocusChanged;
                chatView.MessageAdded += OnMessageAdded;
            }
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

        public void OnMessageReceived(string message)
        {
            chatView?.AddMessage(message);
        }

        private void OnMessageAdded(string message)
        {
            chatInteractor.SendChatMessage(message);
        }

        private void OnFocusChanged(bool isFocused)
        {
            if (focusStateController?.GetFocusState() == UIFocusState.UI)
            {
                return;
            }

            var focusState = isFocused ? UIFocusState.Chat : UIFocusState.Game;
            focusStateController?.ChangeFocusState(focusState);
        }
    }
}