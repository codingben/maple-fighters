using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ChatController : MonoBehaviour
    {
        public event Action<string> MessageSent;

        private IChatView chatView;

        private void Awake()
        {
            CreateChatWindow();
        }

        private void CreateChatWindow()
        {
            chatView = UIElementsCreator.GetInstance().Create<ChatWindow>();
            chatView.MessageAdded += OnMessageAdded;
        }

        private void OnDestroy()
        {
            DestroyChatWindow();
        }

        private void DestroyChatWindow()
        {
            if (chatView != null)
            {
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
            if (chatView != null)
            {
                chatView.AddMessage(message);
            }
        }

        private void OnMessageAdded(string message)
        {
            if (chatView != null)
            {
                chatView.AddMessage(message);
            }

            MessageSent?.Invoke(message);
        }
    }
}