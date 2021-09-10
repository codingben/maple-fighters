using System;
using UnityEngine;

namespace Scripts.Services.ChatApi
{
    public class WebSocketChatApi : MonoBehaviour, IChatApi
    {
        public static WebSocketChatApi GetInstance()
        {
            if (instance == null)
            {
                var chatApi = new GameObject("WebSocket Chat Api");
                instance = chatApi.AddComponent<WebSocketChatApi>();
            }

            return instance;
        }

        private static WebSocketChatApi instance;

        public Action<string> ChatMessageReceived { get; set; }

        public void SendChatMessage(string message)
        {
            // TODO: Implement
        }
    }
}