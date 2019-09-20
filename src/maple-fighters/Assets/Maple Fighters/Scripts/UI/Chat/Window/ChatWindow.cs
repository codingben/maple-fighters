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
            set
            {
                characterName = value;
            }
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
            set
            {
                isTypingMessage = value;

                FocusChanged?.Invoke(isTypingMessage);
            }
        }

        private bool isTypingMessage;
        private string characterName;

        public void AddMessage(string message, ChatMessageColor color = ChatMessageColor.None)
        {
            if (chatText != null)
            {
                if (color != ChatMessageColor.None)
                {
                    var colorName = color.ToString().ToLower();
                    message = $"<color={colorName}>{message}</color>";
                }

                var isEmpty = chatText.text.Length == 0;
                chatText.text += !isEmpty ? $"\n{message}" : $"{message}";
            }
        }

        private void Update()
        {
            if (isTypingMessage)
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
            var isAnySendKeyPressed = IsAnySendKeyPressed();
            var isEscapeKeyPressed = IsEscapeKeyPressed();

            if (isAnySendKeyPressed || isEscapeKeyPressed)
            {
                if (isAnySendKeyPressed)
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

        private void SendMessage()
        {
            if (chatInputField != null)
            {
                var text = chatInputField.text;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    var message = $"{characterName}: {text}";

                    MessageAdded?.Invoke(message);
                }
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