using Chat.Common;
using Scripts.Containers.Service;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class ChatInputFieldController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        private ChatWindow chatWindow;

        private void Awake()
        {
            chatWindow = GetComponent<ChatWindow>();
        }

        private void Update()
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

            if (inputField.text.Length == 0)
            {
                return;
            }

            SendMessage();
            AddMessageToChatAndResetInputFieldText();
        }

        private void SendMessage()
        {
            var message = $"Player: {inputField.text}";
            var parameters = new ChatMessageRequestParameters(message);
            ServiceContainer.ChatService.SendChatMessage(parameters);
        }

        private void AddMessageToChatAndResetInputFieldText()
        {
            var message = $"Player: {inputField.text}";
            chatWindow.AddMessageToChatText.Invoke(message);

            inputField.text = string.Empty;
        }
    }
}