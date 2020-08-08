using System;

namespace Scripts.UI.Chat
{
    public interface IChatView
    {
        event Action<bool> FocusChanged;

        event Action<string> MessageAdded;

        string CharacterName { set; }

        void AddMessage(string message, UIChatMessageColor color = UIChatMessageColor.None);
    }
}