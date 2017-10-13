using System;
using Chat.Common;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
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

        private string characterName;

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
            ResetInputField();
        }

        private void SendMessage()
        {
            if (characterName == null)
            {
                characterName = GetCharacterName();
            }

            var message = $"{characterName}: {inputField.text}";
            SendChatMessage?.Invoke(message);

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

        private string GetCharacterName()
        {
            var localGameObject = GameContainers.GameObjectsContainer.GetLocalGameObject().GetGameObject().AssertNotNull();
            var characterInformation = localGameObject.GetComponent<CharacterInformationProvider>();
            return characterInformation.GetCharacterName();
        }
    }
}