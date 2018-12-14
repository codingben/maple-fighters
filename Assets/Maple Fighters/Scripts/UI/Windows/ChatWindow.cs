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
        public event Action<string> MessageAdded;

        private const KeyCode SendMessageKeyCode = KeyCode.Return;
        private const KeyCode SecondarySendMessageKeyCode = KeyCode.KeypadEnter;
        private const KeyCode CloseMessageKeyCode = KeyCode.Escape;

        [SerializeField]
        private TextMeshProUGUI chatText;

        [SerializeField]
        private TMP_InputField chatInputField;

        private string characterName;

        private void Update()
        {
            // TODO: Remove this from here
            if (FocusController.Instance.Focusable == Focusable.Chat)
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

                // TODO: Remove this from here
                FocusController.Instance.SetState(Focusable.Game);
            }
        }

        private void UnFocusableState()
        {
            if (IsAnySendKeyPressed())
            {
                ActivateOrDeactivateInputField();
                SelectOrDeselectChatInputField();

                // TODO: Remove this from here
                FocusController.Instance.SetState(Focusable.Chat);
            }
        }

        private void SendMessage()
        {
            if (characterName == null)
            {
                characterName = GetCharacterName();
            }

            var text = chatInputField.text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                var message = $"{characterName}: {text}";
                AddMessage(message);

                MessageAdded?.Invoke(message);
            }
        }

        public void AddMessage(string message, ChatMessageColor color = ChatMessageColor.None)
        {
            if (color != ChatMessageColor.None)
            {
                var colorName = color.ToString().ToLower();
                message = $"<color={colorName}>{message}</color>";
            }

            var isEmpty = chatText.text.Length == 0;
            chatText.text += !isEmpty ? $"\n{message}" : $"{message}";
        }

        private void ActivateOrDeactivateInputField()
        {
            var active = !chatInputField.gameObject.activeSelf;

            chatInputField.text = string.Empty;
            chatInputField.gameObject.SetActive(active);
        }

        private void SelectOrDeselectChatInputField()
        {
            var active = chatInputField.gameObject.activeSelf;
            var selected = active ? chatInputField.gameObject : null;
            EventSystem.current.SetSelectedGameObject(selected);
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

        private string GetCharacterName()
        {
            var sceneObject = 
                SceneObjectsContainer.Instance.GetLocalSceneObject()
                    .AssertNotNull();

            var character = 
                sceneObject.GetGameObject()
                    .GetComponent<CharacterInformationProvider>();

            return character.GetCharacterInfo().Name;
        }
    }
}