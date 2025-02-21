using Akka.Actor;
using Akka.Event;
using AkkaWordCounter;

//https://petabridge.com/bootcamp/lessons/unit-0/first-akkadotnet-app/

ActorSystem myActorSystem = ActorSystem.Create("LocalSystem");
myActorSystem.Log.Info("Hello from the ActorSystem");

var counterActor = myActorSystem.ActorOf(Props.Create<CounterActor>(), "CounterActor");
var parserActor = myActorSystem.ActorOf(Props.Create(() => new ParserActor(counterActor)), "ParserActor");

Task<IDictionary<string, int>> completedPromise = counterActor.Ask<IDictionary<string, int>>(
    @ref => new CounterQueries.FetchCounts(@ref),
    null, CancellationToken.None);

parserActor.Tell(new DocumentCommands.ProcessDocument(
    """
    This is a test of the Akka.NET Word Counter.
    I would go to the beach if it wasn't for all of the beach sand there.
    """
    ));

IDictionary<string, int> counts = await completedPromise;
foreach(var kvp in counts)
{
    //going to use string interpolaion here because we don't care about performance
    myActorSystem.Log.Info($"{kvp.Key}: {kvp.Value} instances");
}

await myActorSystem.Terminate();