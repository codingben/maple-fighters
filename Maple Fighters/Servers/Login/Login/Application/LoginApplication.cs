using System;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Login.Application
{
    public class LoginApplication : ApplicationBase
    {
        public LoginApplication(IFiberProvider fiberProvider) 
            : base(fiberProvider)
        {
            // Left blank intentionally
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            throw new NotImplementedException();
        }
    }
}