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
        public event Action<string> SendChatMessage;
        public bool IsChatActive { get; set; }

        private string ChatText
        {
            set
            {
                var time = DateTime.Now.ToString("HH:mm");
                if (ChatText.Length > 0)
                {
                    chatText.text += $"\n<color=white>[{time}]</color> {value}";
                }
                else
                {
                    chatText.text += $"<color=white>[{time}]</color> {value}";
                }
            }
            get
            {
                return chatText.text;
            }
        }

        [SerializeField] private TextMeshProUGUI chatText;
        [SerializeField] private TMP_InputField inputField;
        [Header("Keyboard")]
        [SerializeField] private KeyCode sendMessageKey = KeyCode.Return;
        [SerializeField] private KeyCode sendMessageSecondaryKey = KeyCode.KeypadEnter;

        private string characterName;

        private void Update()
        {
            if (IsFocusable() 
                && (Input.GetKeyDown(sendMessageKey) || Input.GetKeyDown(sendMessageSecondaryKey)))
            {
                OnEnterClicked();
            }
        }

        private void OnEnterClicked()
        {
            SetActiveInputField(IsChatInputFieldActivated());

            if (IsChatActive && !IsChatInputFieldActivated() && !string.IsNullOrWhiteSpace(inputField.text))
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
            ChatText = $"{message}";
        }

        public void AddMessage(string message, ChatMessageColor color)
        {
            var chatMessageColor = color.ToString().ToLower();
            ChatText = $"<color={chatMessageColor}>{message}</color>";
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

        private bool IsFocusable()
        {
            if (FocusController.Instance.Focusable == Focusable.UI)
            {
                if (inputField.interactable)
                {
                    inputField.interactable = false;
                }
                return false;
            }

            if (!inputField.interactable)
            {
                inputField.interactable = true;
            }
            return true;
        }
    }
}