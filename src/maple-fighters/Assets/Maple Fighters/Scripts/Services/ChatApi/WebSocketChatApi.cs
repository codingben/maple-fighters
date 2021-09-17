using System;
using UnityEngine;
using NativeWebSocket;
using System.Text;
using ScriptableObjects.Configurations;

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

        private WebSocket webSocket;

        private async void Start()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                var serverData =
                    networkConfiguration.GetServerData(ServerType.Chat);
                var url = serverData.Url;

                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                webSocket = new WebSocket(url);
                webSocket.OnMessage += OnMessage;

                if (webSocket != null)
                {
                    await webSocket.Connect();
                }
            }
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket?.DispatchMessageQueue();
#endif
        }

        private async void OnDestroy()
        {
            ApiProvider.RemoveChatApiProvider();

            if (webSocket != null)
            {
                await webSocket.Close();
            }
        }

        private async void OnApplicationQuit()
        {
            if (webSocket != null)
            {
                await webSocket.Close();
            }
        }

        public void SendChatMessage(string message)
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                webSocket.SendText(message);
            }
        }

        private void OnMessage(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            ChatMessageReceived?.Invoke(message);
        }
    }
}