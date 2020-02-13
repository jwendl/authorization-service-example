# Building and Running Source

## Creating an Azure AD application registration

```bash
password="$(</dev/urandom tr -dc 'A-Za-z0-9!#$%~' | head -c 32; echo)"
az ad app create --display-name "ApiExample" --homepage "https://jwendl.net/" --identifier-uris "api://api-example" --reply-urls "https://localhost/" --password "$password"
```

## Configuration definitions inside local.settings.json

The goal is to allow for "Clone and F5" functionality. So everything here doesn't include a secret and can safely be added to source control.

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "TokenValidator": {
    "Audience": "api://api-example",
    "TenantId": "72f988bf-86f1-41af-91ab-2d7cd011db47",
    "ClientId": "e16bb3bb-3c32-4bd6-a070-c935e2cdcdcb"
  },
  "Database": {
    "ConnectionString": "Data Source=(LocalDB)\\MSSQLLocalDB;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0;"
  }
}
```
