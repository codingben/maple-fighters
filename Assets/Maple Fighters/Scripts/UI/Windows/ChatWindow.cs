using System;
using Chat.Common;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Windows
{
    public class ChatWindow : UserInterfaceWindow
    {
        public event Action<string> SendChatMessage;
        public Action<ChatMessageEventParameters> ChatMessageReceived;

        [SerializeField] private TextMeshProUGUI chatText;
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            ChatMessageReceived = OnChatMessageReceived;
        }

        private void Update()
        {
            ActivateOrDeactivateInputField();
            DetectTextChangesAndSendMessage();
        }

        private void ActivateOrDeactivateInputField()
        {
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return;
            }

            inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);

            if (inputField.gameObject.activeSelf)
            {
                inputField.ActivateInputField();
            }
        }

        private void DetectTextChangesAndSendMessage()
        {
            if (inputField.gameObject.activeSelf || inputField.text.Length <= 0)
            {
                return;
            }

            SendMessage();
            AddMessageToChatLocally();

            ResetInputField();
        }

        private void SendMessage()
        {
            var message = $"Player: {inputField.text}";
            SendChatMessage?.Invoke(message);
        }

        private void AddMessageToChatLocally()
        {
            var message = $"Player: {inputField.text}";
            AddMessageToChat(message);
        }

        private void AddMessageToChat(string message)
        {
            chatText.text += $"\n{message}";
        }

        private void ResetInputField()
        {
            inputField.text = string.Empty;
        }

        private void OnChatMessageReceived(ChatMessageEventParameters parameters)
        {
            var message = parameters.Message;
            AddMessageToChat(message);
        }
    }
}