using Game.Application.Components;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    public class AnimationStateChangedMessageSender : ComponentBase
    {
        private IAnimationStateProvider animationStateProvider;
        private IGameObjectGetter gameObjectGetter;
        private IMessageSender messageSender;

        protected override void OnAwake()
        {
            animationStateProvider = Components.Get<IAnimationStateProvider>();
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
            animationStateProvider.AnimationStateChanged += OnAnimationStateChanged;
        }

        private void UnsubscribeFromAnimationStateChanged()
        {
            animationStateProvider.AnimationStateChanged -= OnAnimationStateChanged;
        }

        private void OnAnimationStateChanged()
        {
            var id = gameObjectGetter.Get().Id;
            var messageCode = (byte)MessageCodes.AnimationStateChanged;
            var message = new AnimationStateChangedMessage()
            {
                GameObjectId = id,
                AnimationState = animationStateProvider.GetState()
            };

            messageSender.SendMessageToNearbyGameObjects(messageCode, message);
        }
    }
}