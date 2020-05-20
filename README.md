# NSonic

[![Build](https://img.shields.io/azure-devops/build/cyaspik/DotNet/5/master.svg)](https://dev.azure.com/cyaspik/DotNet/_build/latest?definitionId=5)
[![Tests](https://img.shields.io/azure-devops/tests/cyaspik/DotNet/5/master.svg)](https://dev.azure.com/cyaspik/DotNet/_build/latest?definitionId=5)
[![Coverage](https://img.shields.io/azure-devops/coverage/cyaspik/DotNet/5/master.svg)](https://dev.azure.com/cyaspik/DotNet/_build/latest?definitionId=5)
[![NuGet FastRunner](https://img.shields.io/nuget/v/NSonic.svg)](https://www.nuget.org/packages/NSonic/)

NSonic is an open-source .NET client implementation for the [Sonic](https://github.com/valeriansaliou/sonic) search backend.

## Usage

Go and read the [documentation](https://dev.azure.com/cyaspik/DotNet/_wiki/wikis/DotNet.wiki/14/Documentation).

## Changelog

### v1.3

* If an NSonic connection operation fails, it can be safely retried.
* Code improvements.
* Fixed threading issues that would result in deadlocks.

### v1.2

* Implemented asynchronous equivalents to most functionality.
* Changed the versioning scheme to conform to [Semantic Versioning](https://semver.org/).

### v1.1.0.9

* Fixed locale for ingest push and search queries.

### v1.1

* First stable release.

## Contributing

See the [wiki page](https://dev.azure.com/cyaspik/DotNet/_wiki/wikis/DotNet.wiki/12/Development-process) for information
about contributing to the project.
