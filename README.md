# Trico

trico is a simple configuration library for your .net projects

## Status

|            |                                                                                                                                                                                                                                                        |
|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| main       | [![Build Status](https://dev.azure.com/ujinjinjin/Trico/_apis/build/status/Trico?repoName=Ujinjinjin%2Ftrico&branchName=main)](https://dev.azure.com/ujinjinjin/Trico/_build/latest?definitionId=17&repoName=Ujinjinjin%2Ftrico&branchName=main)       |
| develop    | [![Build Status](https://dev.azure.com/ujinjinjin/Trico/_apis/build/status/Trico?repoName=Ujinjinjin%2Ftrico&branchName=develop)](https://dev.azure.com/ujinjinjin/Trico/_build/latest?definitionId=17&repoName=Ujinjinjin%2Ftrico&branchName=develop) |
| unit tests | ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/ujinjinjin/eb9ff178-4bd6-4905-8275-a5f56afdbb33/17)                                                                                                                                    |
| version    | ![Nuget](https://img.shields.io/nuget/v/trico)                                                                                                                                                                                                         |
| downloads  | ![Nuget](https://img.shields.io/nuget/dt/trico)                                                                                                                                                                                                        |
| license    | ![GitHub](https://img.shields.io/github/license/ujinjinjin/trico)                                                                                                                                                                                      |
| size       | ![GitHub repo size](https://img.shields.io/github/repo-size/ujinjinjin/trico)                                                                                                                                                                          |

## Supported providers

|                       |     |
|-----------------------|-----|
| in-memory             | ✓   |
| file                  | ✓   |
| environment variables | ✓   |
| api                   | ✕   |
| database              | ✕   |

## Usage

### Step 1. Add Trico services to the service collection

You start with adding `trico` services to the [`IServiceCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection)

```csharp
serviceCollection.AddConfiguration();
```

### Step 2. Configure required providers

Add required providers to the [`IServiceCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection).

_**Important!**_ Configurations are looked up in providers in the order of registration in service collection.

#### In-Memory provider

In-Memory configuration provider is a non-persistent short-term storage. Used to work with configurations that are set and used in runtime. In coupe with other providers can also be used as a default application configuration\
To add provider to your application, use:

```csharp
serviceProvider.AddInMemoryProvider();
```

Initial set of configurations can be set by passing `Dictionary<string, string>` into the method:

```csharp
Dictionary<string, string> defaultConfigs = GetDefaultConfigs();
serviceProvider.AddInMemoryProvider(defaultConfigs);
```

#### File provider

File configuration provider is a persistent long-term storage.\
To add provider to your application, use:

```csharp
serviceCollection.AddFileProvider();
```

Load params:

| name            | description                                      |
|-----------------|--------------------------------------------------|
| config-filepath | path to the file where configurations are stored |

#### Environment variable provider

Environment variable configuration provider is a non-persistent long-term storage. Allows to import application configurations from environment variables, but can't update them.\
To add provider to your application, use:

```csharp
serviceCollection.AddEnvironmentVariableProvider();
```

Load params:

| name   | description                                                               |
|--------|---------------------------------------------------------------------------|
| prefix | variable name prefix. Used to narrow down the scope of imported variables |

### Step 3. Explicitly load configurations

Load configurations either asynchronously:

```csharp
Dictionary<string, string> options = new Dictionary<string, string>() {
    { "config-filepath", "..." },
    { "prefix", "" },
}
await config.LoadAsync(options, default);
```

### Step 4. Use configurations

```csharp
var value = config["key"];
```

## More info

refer to the [Playground](https://github.com/Ujinjinjin/trico/tree/main/Trico.Playground) project
