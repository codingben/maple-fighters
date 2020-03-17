using Authenticator.API.Services;
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