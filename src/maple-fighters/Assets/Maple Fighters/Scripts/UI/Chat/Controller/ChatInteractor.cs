using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(IOnChatMessageReceived))]
    public class ChatInteractor : MonoBehaviour
    {
        private IGameApi gameApi;
        private IOnChatMessageReceived onChatMessageReceived;

        private string characterName;

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            onChatMessageReceived = GetComponent<IOnChatMessageReceived>();

            SubscribeToGameApiEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromGameApiEvents();
        }

        private void SubscribeToGameApiEvents()
        {
            gameApi?.ChatMessageReceived?.AddListener(OnChatMessageReceived);
        }

        private void UnsubscribeFromGameApiEvents()
        {
            gameApi?.ChatMessageReceived?.RemoveListener(OnChatMessageReceived);
        }

        public void SetCharacterName(string name)
        {
            characterName = name;
        }

        public void SendChatMessage(string message)
        {
            var entity =
                EntityContainer.GetInstance().GetLocalEntity();
            var id =
                entity.Id;
            var chatMessage = new ChatMessage()
            {
                SenderId = id,
                Name = characterName,
                Content = message
            };

            gameApi?.SendMessage(MessageCodes.ChatMessage, chatMessage);
        }

        private void OnChatMessageReceived(ChatMessage chatMessage)
        {
            var content = chatMessage.ContentFormatted;

            onChatMessageReceived.OnMessageReceived(content);
        }
    }
}