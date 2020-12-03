using System;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Authenticator.Infrastructure.Repository;
using Common.MongoDB;
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
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (databaseUrl == null)
            {
                throw new NullReferenceException("Please set DATABASE_URL");
            }

            services.AddSingleton<IDatabaseProvider>(new MongoDatabaseProvider(databaseUrl));
            services.AddSingleton<IAccountRepository, MongoAccountRepository>();
            // services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
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
            });
        }
    }
}