using Authenticator.Application.Peer.Logic.Operations;
using Authenticator.Common.Enums;
using CommonTools.Log;
using CommunicationHelper;
using ServerCommon.PeerLogic.Common;

namespace Authenticator.Application.Peer.Logic
{
    public class MainPeerLogic : InboundPeerLogicBase<AuthenticatorOperations, EmptyEventCode>
    {
        public override void OnSetup()
        {
            base.OnSetup();

            LogUtils.Log("OnCleanup()");

            OperationHandlerRegister.SetHandler(AuthenticatorOperations.Login, new LoginOperationHandler());
        }

        public override void OnCleanup()
        {
            base.OnCleanup();

            LogUtils.Log("OnCleanup()");
        }
    }
}