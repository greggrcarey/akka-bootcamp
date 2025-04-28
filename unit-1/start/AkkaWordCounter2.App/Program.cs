using Akka.Hosting;
using AkkaWordCounter2.App.Actors;
using AkkaWordCounter2.App;
using AkkaWordCounter2.App.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var hostBuilder = new HostBuilder();
//https://petabridge.com/bootcamp/lessons/unit-1/akkadotnet-sagas/

hostBuilder
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appSettings.json", optional: true)
                .AddJsonFile($"appSettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddWordCounterSettings();
        services.AddHttpClient(); // needed for IHttpClientFactory

        services.AddAkka("MyActorSystem", (builder, sp) =>
        {
            builder.ConfigureLoggers(logConfig =>
            {
                logConfig.AddLoggerFactory();
            })
            .AddApplicationActors()
            .AddStartup(async (system, registry) =>
            {
                IOptions<WordCounterSettings>? settings = sp.GetRequiredService<IOptions<WordCounterSettings>>();
                IActorRef? jobActor = await registry.GetAsync<WordCountJobActor>();
                AbsoluteUri[]? absoluteUris = [.. settings.Value.DocumentUris.Select(uri => new AbsoluteUri(new Uri(uri)))];
                jobActor.Tell(new DocumentCommands.ScanDocuments(absoluteUris));

                // wait for the job to complete
                var counts = await jobActor.Ask<DocumentEvents.CountsTabulatedForDocuments>(DocumentQueries.SubscribeToAllCounts.Instance, TimeSpan.FromMinutes(1));

                foreach (var (word, count) in counts.WordFrequencies)
                {
                    Console.WriteLine($"Word count for {word}: {count}");
                }
            });
        });
    });
    

var host = hostBuilder.Build();

await host.RunAsync();