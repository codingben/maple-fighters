using System;

namespace Scripts.UI.Chat
{
    public interface IChatView
    {
        event Action<bool> FocusChanged;

        event Action<string> MessageAdded;

        bool IsTypingBlocked { get; }

        void AddMessage(string message, UIChatMessageColor color = UIChatMessageColor.None);

        void BlockOrUnblockTyping(bool block);
    }
}