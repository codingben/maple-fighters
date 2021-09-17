using System;
using UnityEngine;

namespace Scripts.Services.ChatApi
{
    public class DummyChatApi : MonoBehaviour, IChatApi
    {
        public static DummyChatApi GetInstance()
        {
            if (instance == null)
            {
                var chatApi = new GameObject("Dummy Chat Api");
                instance = chatApi.AddComponent<DummyChatApi>();
            }

            return instance;
        }

        private static DummyChatApi instance;

        public Action<string> ChatMessageReceived { get; set; }

        public void SendChatMessage(string message)
        {
            // Left blank intentionally
        }

        private void OnDestroy()
        {
            ApiProvider.RemoveChatApiProvider();
        }
    }
}