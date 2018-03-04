using System;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Windows
{
    public class ChatWindow : UserInterfaceWindow
    {
        public bool IsChatActive { get; set; }
        public event Action<string> SendChatMessage;

        [SerializeField] private TextMeshProUGUI chatText;
        [SerializeField] private TMP_InputField inputField;
        [Header("Keyboard")]
        [SerializeField] private KeyCode sendMessageKey = KeyCode.Return;
        [SerializeField] private KeyCode sendMessageSecondaryKey = KeyCode.KeypadEnter;

        private string characterName;

        private void Update()
        {
            if (Input.GetKeyDown(sendMessageKey) || Input.GetKeyDown(sendMessageSecondaryKey))
            {
                OnEnterClicked();
            }
        }

        private void OnEnterClicked()
        {
            SetActiveInputField(IsChatInputFieldActivated());

            if (IsChatActive && !IsChatInputFieldActivated() && !string.IsNullOrWhiteSpace(chatText.text))
            {
                SendMessage();
                ResetInputField();
            }

            FocusController.Instance.Focusable = IsChatInputFieldActivated() ? Focusable.Chat : Focusable.Game;
        }

        private void SendMessage()
        {
            if (characterName == null)
            {
                characterName = GetCharacterName();
            }

            var message = $"{characterName}: {inputField.text}";
            SendChatMessage?.Invoke(message);

            AddMessage(message);
        }

        public void AddMessage(string message)
        {
            if (chatText.text.Length > 0)
            {
                chatText.text += $"\n{message}";
            }
            else
            {
                chatText.text += $"{message}";
            }
        }

        public void AddMessage(string message, ChatMessageColor color)
        {
            if (chatText.text.Length > 0)
            {
                var chatMessageColor = color.ToString().ToLower();
                chatText.text += $"\n<color={chatMessageColor}>{message}</color>";
            }
            else
            {
                var chatMessageColor = color.ToString().ToLower();
                chatText.text += $"<color={chatMessageColor}>{message}</color>";
            }
        }

        private void SetActiveInputField(bool active)
        {
            inputField.gameObject.SetActive(!active);

            EventSystem.current.SetSelectedGameObject(IsChatInputFieldActivated() ? inputField.gameObject : null);
        }

        private void ResetInputField()
        {
            inputField.text = string.Empty;
        }

        private string GetCharacterName()
        {
            var localGameObject = SceneObjectsContainer.Instance.GetLocalSceneObject().GetGameObject().AssertNotNull();
            var characterInformation = localGameObject.GetComponent<CharacterInformationProvider>();
            var name = characterInformation.GetCharacterInfo().Name;
            return name;
        }

        private bool IsChatInputFieldActivated()
        {
            return inputField.gameObject.activeSelf;
        }
    }
}