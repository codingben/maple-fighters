using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace Authentication.Application.Peer.Logic.Operations
{
    public class LoginOperationHandler : IOperationRequestHandler<EmptyParameters, EmptyParameters>
    {
        public EmptyParameters? Handle(
            MessageData<EmptyParameters> messageData,
            ref MessageSendOptions sendOptions)
        {
            LogUtils.Log("LoginOperationHandler()");

            return new EmptyParameters();
        }
    }
}