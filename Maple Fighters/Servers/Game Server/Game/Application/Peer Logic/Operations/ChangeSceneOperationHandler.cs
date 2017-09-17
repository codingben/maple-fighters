using CommonCommunicationInterfaces;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ChangeSceneOperationHandler : IOperationRequestHandler<ChangeSceneRequestParameters, EmptyParameters>
    {
        private readonly IGameObject gameObject;

        public ChangeSceneOperationHandler(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public EmptyParameters? Handle(MessageData<ChangeSceneRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var sceneId = messageData.Parameters.Map;
            gameObject.ChangeScene(sceneId);
            return new EmptyParameters();
        }
    }
}