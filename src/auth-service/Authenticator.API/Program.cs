using Authenticator.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host.CreateDefaultBuilder()
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddConsole();
    })
    .ConfigureWebHostDefaults(builder =>
    {
        builder.UseStartup<AuthenticatorStartup>();
    })
    .Build()
    .Run();