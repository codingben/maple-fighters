using System;
using Chat.Common;
using Scripts.Containers.Service;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class ChatWindow : UserInterfaceWindow
    {
        public Action<string> AddMessageToChatText;

        [SerializeField] private TextMeshProUGUI chatText;

        private void Awake()
        {
            AddMessageToChatText = OnAddMessageToChatText;
            ServiceContainer.ChatService.ChatMessageReceived.AddListener(OnChatMessageReceived);
        }

        private void OnChatMessageReceived(ChatMessageEventParameters parameters)
        {
            var message = parameters.Message;
            chatText.text += $"\n{message}";
        }

        private void OnAddMessageToChatText(string message)
        {
            chatText.text += $"\n{message}";
        }
    }
}