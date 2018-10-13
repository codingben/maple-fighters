using System;
using Authenticator.Common.Enums;
using Authenticator.Common.Parameters;
using Authenticator.Domain.Aggregates.User.Services;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Authenticator.Application.Peer.Logic.Operations
{
    public class LoginOperationHandler : 
        IOperationRequestHandler<LoginRequestParameters, LoginResponseParameters>
    {
        private readonly ILoginService loginService;

        public LoginOperationHandler(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public LoginResponseParameters? Handle(
            MessageData<LoginRequestParameters> messageData,
            ref MessageSendOptions sendOptions)
        {
            // TODO: Parameters validator
            var email = messageData.Parameters.Email;
            var password = messageData.Parameters.Password;

            var authenticationStatus = loginService.Authenticate(email, password);
            LoginStatus loginStatus;

            switch (authenticationStatus)
            {
                case AuthenticationStatus.Authenticated:
                {
                    loginStatus = LoginStatus.Succeed;
                    break;
                }

                case AuthenticationStatus.NotFound:
                {
                    loginStatus = LoginStatus.WrongEmail;
                    break;
                }

                case AuthenticationStatus.WrongPassword:
                {
                    loginStatus = LoginStatus.WrongPassword;
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return new LoginResponseParameters(loginStatus);
        }
    }
}