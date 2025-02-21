using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaWordCounter;

public static class DocumentCommands
{
    public sealed record ProcessDocument(string RawText);
}

// Counter Inputs
public static class CounterCommands
{
    public sealed record CountTokens(IReadOnlyList<string> Tokens);

    // parser reached the end of the document
    public sealed record ExpectNoMoreTokens();
}

// Counter Queries
public static class CounterQueries
{
    // Send this actor a notification once counting is complete
    public sealed record FetchCounts(IActorRef Subscriber);
}



