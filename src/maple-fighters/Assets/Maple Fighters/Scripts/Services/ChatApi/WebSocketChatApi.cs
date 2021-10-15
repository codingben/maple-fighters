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

        private void Start()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                var url =
                    networkConfiguration.GetServerUrl(ServerType.Chat);

                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                try
                {
                    webSocket = new WebSocket(url);
                    webSocket.OnMessage += OnMessage;
                    webSocket.Connect();
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
            }
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket?.DispatchMessageQueue();
#endif
        }

        private void OnDestroy()
        {
            ApiProvider.RemoveChatApiProvider();

            try
            {
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    webSocket.Close();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        public void SendChatMessage(string message)
        {
            try
            {
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    webSocket.SendText(message);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        private void OnMessage(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            ChatMessageReceived?.Invoke(message);
        }
    }
}