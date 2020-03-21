using Authenticator.API.Controllers;
using Authenticator.API.Services;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Authenticator.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authenticator.API
{
    public class AuthenticatorStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddTransient<ILoginController, LoginController>();
            services.AddTransient<IRegistrationController, RegistrationController>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }

        public void Configure(
            IApplicationBuilder applicationBuilder,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseRouting();
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AuthenticatorService>();
                endpoints.MapGet(
                    "/", 
                    async context => await context.Response.WriteAsync("Please use a gRPC client."));
            });
        }
    }
}