using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ChatController : MonoBehaviour
    {
        public event Action<string> MessageSent;

        private ChatWindow chatWindow;

        private void Awake()
        {
            CreateChatWindow();
        }

        private void CreateChatWindow()
        {
            chatWindow = UIElementsCreator.GetInstance().Create<ChatWindow>();
            chatWindow.MessageAdded += OnMessageAdded;
        }

        private void OnDestroy()
        {
            DestroyChatWindow();
        }

        private void DestroyChatWindow()
        {
            if (chatWindow != null)
            {
                chatWindow.MessageAdded -= OnMessageAdded;

                Destroy(chatWindow.gameObject);
            }
        }

        public void SetCharacterName(string name)
        {
            if (chatWindow != null)
            {
                chatWindow.SetCharacterName(name);
            }
        }

        public void OnMessageReceived(string message)
        {
            if (chatWindow != null)
            {
                chatWindow.AddMessage(message);
            }
        }

        private void OnMessageAdded(string message)
        {
            if (chatWindow != null)
            {
                chatWindow.AddMessage(message);
            }

            MessageSent?.Invoke(message);
        }
    }
}