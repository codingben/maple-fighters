using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authenticator.API
{
    public class Program
    {
        public static void Main() => CreateHostBuilder().Build().Run();

        /*
         * Additional configuration is required to successfully run gRPC on macOS.
         * For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        */
        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.AddConsole();
                })
                .ConfigureWebHostDefaults(x => x.UseStartup<AuthenticatorStartup>());
    }
}