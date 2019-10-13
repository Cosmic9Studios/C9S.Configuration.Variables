# C9S.Extensions.Configuration


### Usage
---

This

```json
{
    "General":
    {
        "HttpListenPort": "3000",
        "HttpsListenPort": "3001",
        "WebUrl": "https://localhost:{{General.HttpsListenPort}}"
    },
    "Pages":
    {
        "AuthorizeAddress": "{{General.WebUrl}}/api/beta/authorize",
        "DashboardAddress": "{{General.WebUrl}}/dashboard"
    }
}
```

Becomes

```json
{
    "General":
    {
        "HttpsListenPort": "3001",
        "WebUrl": "https://localhost:3001"
    },
    "Pages":
    {
        "AuthorizeAddress": "https://localhost:3001/api/beta/authorize",
        "DashboardAddress": "https://localhost:3001/dashboard"
    }
}
```

After running

Startup.cs

```cs
public void ConfigureServices(IServiceCollection services)
{
    ...

    Configuration.ResolveVariables();

    ...
}
```

ResolveVariables will take any variable path and resolve it. It even works across .json files. As long as the data is stored inside the ConfigurationRoot, it is resolvable

## Variable Identification

In the ResolveVariables function you can declare what a variable is (default `{{variable}}`). 

Example: 

Startup.cs

```cs
public void ConfigureServices(IServiceCollection services)
{
    ...

    Configuration.ResolveVariables("${", "}");

    ...
}
```

appsettitngs.json

```json
{
    "General":
    {
        "HttpListenPort": "3000",
        "HttpsListenPort": "3001",
        "WebUrl": "https://localhost:${General.HttpsListenPort}"
    },
    "Pages":
    {
        "AuthorizeAddress": "${General.WebUrl}/api/beta/authorize",
        "DashboardAddress": "${General.WebUrl}/dashboard"
    }
}
```

Now anything inside the `${variable}` is considered a variable.

## Nested variables (variables inside variables)

If the need arises you can also resolve variables within other variables

Example: 

Assuming `ASPNETCORE_ENVIRONMENT = Development`

appsettings.json

```json
{
    "App":
    {
        "Development": {
            "ClientId": "FooId"
        }
    },
    "Auth":
    {
        "ClientID": "${App.${ASPNETCORE_ENVIRONMENT}.ClientId}"
    }
}
```

Will become

```json
{
    "App":
    {
        "Development": {
            "ClientId": "FooId"
        }
    },
    "Auth":
    {
        "ClientID": "${App.Development.ClientId}"
    }
}
```

Which will resolve to 

```json
{
    "App":
    {
        "Development": {
            "ClientId": "FooId"
        }
    },
    "Auth":
    {
        "ClientID": "FooId"
    }
}
```