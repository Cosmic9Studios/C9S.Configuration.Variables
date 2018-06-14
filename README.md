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

```cs
IConfigurationRoot configuration = ...
configuration.ResolveVariables();
```

ResolveVariables will take any variable path and resolve it. It even works across .json files. As long as the data is stored inside the ConfigurationRoot, it is resolvable