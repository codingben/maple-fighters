using System;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Authenticator.Infrastructure.MongoRepository;
using Authenticator.Infrastructure.InMemoryRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Authenticator.API
{
    public class AuthenticatorStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (databaseUrl != null)
            {
                services.AddSingleton<IDatabaseProvider>(new MongoDatabaseProvider(databaseUrl));
                services.AddSingleton<IAccountRepository, MongoAccountRepository>();
            }
            else
            {
                Console.WriteLine("DATABASE_URL is not set. In memory account repository will be used.");

                services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
            }

            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddControllers();
        }

        public void Configure(
            IApplicationBuilder applicationBuilder,
            IWebHostEnvironment environment,
            ILogger<AuthenticatorStartup> logger)
        {
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseRouting();
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz");
            });
        }
    }
}