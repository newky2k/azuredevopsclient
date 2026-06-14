# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Two NuGet libraries for Azure DevOps REST API access:

- **LoDaTek.AzureDevOps.Services.Client** — standalone, lightweight client for REST APIs *missing* from the official SDK (feeds, NuGet packages, secure files, some agile/work-item bits). Depends only on `Microsoft.TeamFoundation*` task/server packages + `System.Text.Json`. This is the lower layer.
- **LoDaTek.AzureDevOps.Client** — wrapper over the *official* `Microsoft.VisualStudio.Services.*` SDK clients (Git, TFVC, WorkItem, Wiki, Build, Release, Pipelines, etc.) plus a project reference to Services.Client. `AzureDevOpsProvider` exposes all official clients lazily and adds builders/extensions (`WiqlBuilder`, work-item extensions). This is the higher layer.

`SampleWPF` (net9.0-windows) and `TestApp` (net9.0 console) are example consumers, not shipped.

## Build / Run

No `.sln` file. Build per-project or the whole folder:

```
dotnet build LoDaTek.AzureDevOps.Services.Client/LoDaTek.AzureDevOps.Services.Client.csproj
dotnet build LoDaTek.AzureDevOps.Client/LoDaTek.AzureDevOps.Client.csproj
dotnet run --project TestApp        # console smoke test
```

Both libraries multi-target `netstandard2.0;net9.0;net10.0`. The `netstandard2.0` target gets extra `System.Net.Http.Json` / `System.Text.Json` package references (see conditional `ItemGroup` in Services.Client csproj) — preserve that when touching JSON code so netstandard keeps compiling.

No test project exists.

## Architecture (Services.Client)

Read these together to understand the connection model:

- `Bases/DevOpsConnectionBase.cs` — abstract base holding org name + PAT. Subclasses (`Connections/AzureDevOpsCloudConnection`, `AzureDevOpsLegacyCloudConnection`, `AzureDevOpsServerConnection`) only differ by the URL properties they expose (`CommonUrl`, `FeedsApiUrl`, `NugetPackagesApiUrl`, `BaseUrl`) and `ServerType`. To support a new host shape, add a connection subclass — don't branch on URLs elsewhere.
- `GetClient<T>()` / `GetClientAsync<T>()` use **reflection** to construct `DevOpsHttpClientBase` subclasses via their **internal constructor**, then call `TryConnect`. This is why HTTP client ctors are `internal` and take a `DevOpsConnectionBase` — keep that ctor signature or reflection breaks.
- `Bases/DevOpsHttpClientBase.cs` — each client declares an `ApiType` (`Feeds`/`Nuget`/`Common`, see `Enums/ApiType.cs`) which selects the base URL from the connection. Auth is PAT as HTTP Basic: base64 of `":" + PAT`. `TryConnect` GETs the client's `TestUrl` and treats only 404 as failure. A fresh `HttpClient` is built per request via the `Client` property.

Concrete clients: `FeedManagmentHttpClient`, `PackageManagementHttpClient`, `SecureFilesHttpClient`. DTOs live in `Models/`, custom exceptions in `Exceptions/` (`ConnectionFailureException`, `PATPermissionDeniedException`, `RequestFailureException`).

## Architecture (Client wrapper)

`AzureDevOpsProvider` (`LoDaTek.AzureDevOps.Client/AzureDevOpsProvider.cs`) is constructed from a `DevOpsConnectionBase` and lazily creates each official SDK client off a shared `VssConnection` (built from `connection.CommonUrl` + `VssBasicCredential`). The Services.Client clients (feed/package/secure-files) are also surfaced here, so consumers use one provider for both official and missing APIs. This file uses `Newtonsoft.Json` (official SDK contract) — distinct from Services.Client which uses `System.Text.Json`.

## Conventions

- XML doc comments on all public/internal members (often GhostDoc-style boilerplate) — match the existing density.
- British spelling in identifiers (`OrganisationName`, `DevOpsOrganisation`).
- Regions (`#region Fields/Properties/Constructors/Methods`) structure every class.
