using System;
using UnityEngine;

namespace Scripts.Services.ChatApi
{
    public class ChatApi : MonoBehaviour, IChatApi
    {
        public Action<string> ChatMessageReceived { get; set; }

        public void SendChatMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}