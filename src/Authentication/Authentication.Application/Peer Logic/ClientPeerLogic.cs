using Authentication.Common.Enums;
using CommonTools.Log;
using CommunicationHelper;
using ServerCommon.PeerLogic.Common;

namespace Authentication.Application.PeerLogic
{
    public class ClientPeerLogic : ClientPeerLogicBase<AuthenticationOperations, EmptyEventCode>
    {
        public override void OnSetup()
        {
            base.OnSetup();

            LogUtils.Log("OnCleanup()");

            OperationHandlerRegister.SetHandler(AuthenticationOperations.Login, new LoginOperationHandler());
        }

        public override void OnCleanup()
        {
            base.OnCleanup();

            LogUtils.Log("OnCleanup()");
        }
    }
}