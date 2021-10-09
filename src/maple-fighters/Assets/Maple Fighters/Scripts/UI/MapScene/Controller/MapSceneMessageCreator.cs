using System;
using UI;
using UnityEngine;

namespace Scripts.UI.MapScene
{
    public class MapSceneMessageCreator : MonoBehaviour
    {
        [SerializeField]
        private string messageText;

        [SerializeField]
        private float seconds = 2;

        private void Awake()
        {
            CreateMessage(messageText, seconds, OnMessageTimeUp);
        }

        private void OnMessageTimeUp()
        {
            Destroy(gameObject);
        }

        private void CreateMessage(string message, float seconds, Action onTimeUp = null)
        {
            var messageView = UICreator
                .GetInstance()
                .Create<MessageText>();

            if (messageView != null)
            {
                messageView.Text = message;
                messageView.Seconds = seconds;
            }

            if (onTimeUp != null)
            {
                messageView.TimeUp += onTimeUp;
            }
        }
    }
}