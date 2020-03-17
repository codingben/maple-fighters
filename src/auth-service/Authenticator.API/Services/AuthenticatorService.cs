using System.Threading.Tasks;
using Grpc.Core;

namespace Authenticator.API.Services
{
    public class AuthenticatorService : Authenticator.AuthenticatorBase
    {
        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            return base.Login(request, context);
        }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            return base.Register(request, context);
        }
    }
}