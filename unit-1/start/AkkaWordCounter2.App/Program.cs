using Akka.Hosting;
using AkkaWordCounter2.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = new HostBuilder();

hostBuilder.ConfigureServices((context, services) =>
    {
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