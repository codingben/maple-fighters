using Chat.Common;
using Scripts.Containers.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ChatInputFieldController : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
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

            if (inputField.text.Length == 0)
            {
                return;
            }

            SendMessage();
            AddMessageToChatAndRestInputFieldText();
        }

        private void SendMessage()
        {
            var message = $"Player: {inputField.text}";
            var parameters = new ChatMessageRequestParameters(message);
            ServiceContainer.ChatService.SendChatMessage(parameters);
        }

        private void AddMessageToChatAndRestInputFieldText()
        {
            var message = $"Player: {inputField.text}";
            chatWindow.AddMessageToChatText.Invoke(message);

            inputField.text = string.Empty;
        }
    }
}