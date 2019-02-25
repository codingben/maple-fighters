using System;

namespace Scripts.UI.Windows
{
    public interface IChatView
    {
        event Action<string> MessageAdded;

        string CharacterName { set; }

        void AddMessage(string message, ChatMessageColor color = ChatMessageColor.None);
    }
}