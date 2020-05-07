# NSonic

[![Build](https://img.shields.io/azure-devops/build/cyaspik/DotNet/5.svg)](https://dev.azure.com/cyaspik/DotNet/_build/latest?definitionId=5)
[![Tests](https://img.shields.io/azure-devops/tests/cyaspik/DotNet/5.svg)](https://dev.azure.com/cyaspik/DotNet/_build/latest?definitionId=5)
[![Coverage](https://img.shields.io/azure-devops/coverage/cyaspik/DotNet/5.svg)](https://dev.azure.com/cyaspik/DotNet/_build/latest?definitionId=5)
[![NuGet FastRunner](https://img.shields.io/nuget/v/NSonic.svg)](https://www.nuget.org/packages/NSonic/)

NSonic is an open-source .NET client implementation for the [Sonic](https://github.com/valeriansaliou/sonic) search backend.

## Usage

### Search mode

```C#
using (var search = NSonicFactory.Search(hostname, port, secret))
{
	search.Connect();

	var queryResults = search.Query("messages", "user:1", "s");
	Console.WriteLine($"QUERY: {string.Join(", ", queryResults)}");

	var suggestResults = search.Suggest("messages", "user:1", "s");
	Console.WriteLine($"SUGGEST: {string.Join(", ", suggestResults)}");
}
```

### Ingest mode

```C#
using (var ingest = NSonicFactory.Ingest(hostname, port, secret))
{
	ingest.Connect();

	ingest.Push("messages", "user:1", "conversation:1", "This is an example push.", locale: null);

	var popResult = ingest.Pop("messages", "user:1", "conversation:1", "This is an example push.");
	Console.WriteLine($"POP: {popResult}");

	var countResult = ingest.Count("messages", "user:1");
	Console.WriteLine($"COUNT: {countResult}");

	var flushCollectionResult = ingest.FlushCollection("messages");
	Console.WriteLine($"FLUSHC: {flushCollectionResult}");

	var flushBucketResult = ingest.FlushBucket("messages", "user:1");
	Console.WriteLine($"FLUSHB: {flushBucketResult}");

	var flushObjectResult = ingest.FlushObject("messages", "user:1", "conversation:1");
	Console.WriteLine($"FLUSHO: {flushObjectResult}");
}
```

### Control mode

```C#
using (var control = NSonicFactory.Control(hostname, port, secret))
{
	control.Connect();

	var info = control.Info();
	Console.WriteLine($"INFO: {info}");

	control.Trigger("consolidate");
}
```

## Changelog

### v1.2

* Implemented asynchronous equivalents to most functionality.
* Changed the versioning scheme to conform to Semantic Versioning.

### v1.1.0.9

* Fixed locale for ingest push and search queries.

### v1.1

* First stable release.

