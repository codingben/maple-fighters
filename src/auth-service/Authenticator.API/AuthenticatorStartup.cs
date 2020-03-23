using Authenticator.API.Controllers;
using Authenticator.API.Services;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Authenticator.Infrastructure.Repository;
using Common.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authenticator.API
{
    public class AuthenticatorStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            // services.AddSingleton<IDatabaseProvider>(new MongoDatabaseProvider(url: "mongodb://localhost:27017/maple_fighters"));
            // services.AddSingleton<IAccountRepository, MongoAccountRepository>();
            services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
            services.AddTransient<ILoginController, LoginController>();
            services.AddTransient<IRegistrationController, RegistrationController>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }

        public void Configure(
            IApplicationBuilder applicationBuilder,
            IWebHostEnvironment environment,
            ILogger<AuthenticatorStartup> logger)
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
                    async c => await c.Response.WriteAsync("Please use a gRPC client."));
            });

            logger.LogInformation("AuthenticatorStartup::Configure()");
        }
    }
}