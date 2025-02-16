using Akka.Actor;
using Akka.Event;
using AkkaWordCounter;

//https://petabridge.com/bootcamp/lessons/unit-0/first-akkadotnet-app/

ActorSystem myActorSystem = ActorSystem.Create("LocalSystem");
myActorSystem.Log.Info("Hello from the ActorSystem");

//Props == formula for creating an actor
Props myProps = Props.Create<HelloActor>();

//IActorRef == handle for messaging an actor
//Survives actor restarts, is seralizable
IActorRef myActor = myActorSystem.ActorOf(myProps, "MyActor");

//tell my actor to display a message via fire-and-forget messaging
myActor.Tell("Hello, World!");

//use Ask<T> to do request-response messaging
string whatsup = await myActor.Ask<string>("What's up?");
Console.WriteLine(whatsup);


await myActorSystem.Terminate();