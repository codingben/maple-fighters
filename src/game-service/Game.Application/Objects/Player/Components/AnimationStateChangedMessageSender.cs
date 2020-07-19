using Common.ComponentModel;
using Game.Application.Messages;

namespace Game.Application.Objects.Components
{
    public class AnimationStateChangedMessageSender : ComponentBase
    {
        private IAnimationData animationData;
        private IGameObjectGetter gameObjectGetter;
        private IMessageSender messageSender;

        protected override void OnAwake()
        {
            animationData = Components.Get<IAnimationData>();
            gameObjectGetter = Components.Get<IGameObjectGetter>();
            messageSender = Components.Get<IMessageSender>();

            SubscribeToAnimationStateChanged();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromAnimationStateChanged();
        }

        private void SubscribeToAnimationStateChanged()
        {
            animationData.AnimationStateChanged += OnAnimationStateChanged;
        }

        private void UnsubscribeFromAnimationStateChanged()
        {
            animationData.AnimationStateChanged -= OnAnimationStateChanged;
        }

        private void OnAnimationStateChanged()
        {
            var id = gameObjectGetter.Get().Id;
            var messageCode = (byte)MessageCodes.AnimationStateChanged;
            var message = new AnimationStateChangedMessage()
            {
                GameObjectId = id,
                AnimationState = animationData.GetAnimationState()
            };

            messageSender.SendMessageToNearbyGameObjects(messageCode, message);
        }
    }
}