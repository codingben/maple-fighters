using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UICanvasGroup))]
    public class ChatWindow : UIElement
    {
        public event Action<string> MessageAdded;

        public event Action Focused;

        public event Action UnFocused;

        private const KeyCode SendMessageKeyCode = KeyCode.Return;
        private const KeyCode SecondarySendMessageKeyCode = KeyCode.KeypadEnter;
        private const KeyCode CloseMessageKeyCode = KeyCode.Escape;

        [SerializeField]
        private TextMeshProUGUI chatText;

        [SerializeField]
        private TMP_InputField chatInputField;

        private string characterName;
        private bool isTypingMessage;

        public void SetCharacterName(string name)
        {
            characterName = name;
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

                isTypingMessage = false;

                UnFocused?.Invoke();
            }
        }

        private void UnFocusableState()
        {
            if (IsAnySendKeyPressed())
            {
                isTypingMessage = true;

                ActivateOrDeactivateInputField();
                SelectOrDeselectChatInputField();

                Focused?.Invoke();
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

                    AddMessage(message);

                    MessageAdded?.Invoke(message);
                }
            }
        }

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

        private void ActivateOrDeactivateInputField()
        {
            if (chatInputField != null)
            {
                var active = !chatInputField.gameObject.activeSelf;

                chatInputField.text = string.Empty;
                chatInputField.gameObject.SetActive(active);
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