using Authenticator.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host.CreateDefaultBuilder()
    .ConfigureLogging(x =>
    {
        x.ClearProviders();
        x.AddConsole();
    })
    .ConfigureWebHostDefaults(x => x.UseStartup<AuthenticatorStartup>())
    .Build()
    .Run();