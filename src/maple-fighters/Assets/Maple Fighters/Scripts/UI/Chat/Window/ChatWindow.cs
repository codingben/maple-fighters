using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(UICanvasGroup))]
    public class ChatWindow : UIElement, IChatView
    {
        public event Action<bool> FocusChanged;

        public event Action<string> MessageAdded;

        public string CharacterName
        {
            set => characterName = value;
        }

        private const KeyCode SendMessageKeyCode = KeyCode.Return;
        private const KeyCode SecondarySendMessageKeyCode = KeyCode.KeypadEnter;
        private const KeyCode CloseMessageKeyCode = KeyCode.Escape;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI chatText;

        [SerializeField]
        private TMP_InputField chatInputField;

        private bool IsTypingMessage
        {
            get => isTypingMessage;

            set
            {
                isTypingMessage = value;

                FocusChanged?.Invoke(isTypingMessage);
            }
        }

        private bool isTypingMessage;
        private string characterName;

        private void Update()
        {
            if (IsTypingMessage)
            {
                FocusableState();
            }
            else
            {
                UnFocusableState();
            }
        }

        private void FocusableState()
        {
            if (IsAnySendKeyPressed() || IsEscapeKeyPressed())
            {
                if (IsAnySendKeyPressed())
                {
                    SendMessage();
                }

                ActivateOrDeactivateInputField();
                SelectOrDeselectChatInputField();

                IsTypingMessage = false;
            }
        }

        private void UnFocusableState()
        {
            if (IsAnySendKeyPressed())
            {
                IsTypingMessage = true;

                ActivateOrDeactivateInputField();
                SelectOrDeselectChatInputField();
            }
        }

        public void AddMessage(string message, UIChatMessageColor color = UIChatMessageColor.None)
        {
            if (chatText != null)
            {
                if (color != UIChatMessageColor.None)
                {
                    var colorName = color.ToString().ToLower();
                    message = $"<color={colorName}>{message}</color>";
                }

                var isEmpty = chatText.text.Length == 0;
                chatText.text += !isEmpty ? $"\n{message}" : $"{message}";
            }
        }

        private void SendMessage()
        {
            var text = chatInputField?.text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                MessageAdded?.Invoke($"{characterName}: {text}");
            }
        }

        private void ActivateOrDeactivateInputField()
        {
            if (chatInputField != null)
            {
                var isActive = chatInputField.gameObject.activeSelf;

                chatInputField.text = string.Empty;
                chatInputField.gameObject.SetActive(!isActive);
            }
        }

        private void SelectOrDeselectChatInputField()
        {
            if (chatInputField != null)
            {
                var active = chatInputField.gameObject.activeSelf;
                var selected = active ? chatInputField.gameObject : null;

                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(selected);
                }
            }
        }

        private bool IsAnySendKeyPressed()
        {
            return Input.GetKeyDown(SendMessageKeyCode)
                   || Input.GetKeyDown(SecondarySendMessageKeyCode);
        }

        private bool IsEscapeKeyPressed()
        {
            return Input.GetKeyDown(CloseMessageKeyCode);
        }
    }
}