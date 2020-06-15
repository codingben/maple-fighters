using Common.ComponentModel;
using Game.Application.Messages;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class AnimationStateChangedMessageSender : ComponentBase
    {
        private IMessageSender messageSender;
        private IAnimationData animationData;
        private IGameObject gameObject;

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();
            animationData = Components.Get<IAnimationData>();

            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            gameObject = gameObjectGetter.Get();

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
            var message = new AnimationStateChangedMessage()
            {
                GameObjectId = gameObject.Id,
                AnimationState = animationData.GetAnimationState()
            };

            messageSender.SendMessageToNearbyGameObjects((byte)MessageCodes.AnimationStateChanged, message);
        }
    }
}