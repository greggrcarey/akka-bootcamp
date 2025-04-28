using Akka.Hosting;
using AkkaWordCounter2.App.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = new HostBuilder();
//https://petabridge.com/bootcamp/lessons/unit-1/akkadotnet-sagas/

hostBuilder
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appSettings.json", optional: true)
                .AddJsonFile($"appSettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
    }).ConfigureServices((context, services) =>
    {
        services.AddWordCounterSettings();
        services.AddHttpClient(); // needed for IHttpClientFactory

        services.AddAkka("MyActorSystem", (builder, sp) =>
        {
            builder.ConfigureLoggers(logConfig =>
            {
                logConfig.AddLoggerFactory();
            });
        });
    });

var host = hostBuilder.Build();

await host.RunAsync();