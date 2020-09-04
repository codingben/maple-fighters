using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Authenticator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly AuthenticationDataValidator authenticationDataValidator;

        public AuthController(ILoginService loginService)
        {
            this.loginService = loginService;

            authenticationDataValidator = new AuthenticationDataValidator();
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AuthenticationStatus> Login(AuthenticationData authenticationData)
        {
            AuthenticationStatus authenticationStatus;

            var validationResult =
                authenticationDataValidator.Validate(authenticationData);
            if (validationResult.IsValid)
            {
                var email = authenticationData.Email;
                var password = authenticationData.Password;

                authenticationStatus =
                    loginService.Authenticate(email, password);
            }
            else
            {
                var errorMessage = validationResult.ToString();

                return BadRequest(errorMessage);
            }

            return Ok(authenticationStatus);
        }
    }
}