# Policy Manager Architecture

This service provides CRUD functionality for the Thing, ThingAttribute and ThingPolicy entities. 

The seervice also provides an action to checkAccess which utilizes the BEARER token, Microsoft Graph and other data points inside ThingAttribute and ThingPolicy to determine if a specific request has access or not.

It will return a list of valid or invalid policies that were performed during checkAccess.

## Project Structure

```
.
├── PolicyManager
│   ├── Properties
│   ├── Resources
│   ├── Validators
│   ├── bin
│   └── obj
├── PolicyManager.Client
│   ├── Extensions
│   ├── bin
│   └── obj
├── PolicyManager.DataAccess
│   ├── Extensions
│   ├── Functions
│   ├── Interfaces
│   ├── Models
│   ├── Repositories
│   ├── Validators
│   ├── bin
│   └── obj
└── PolicyManager.DataAccess.Tests
    ├── Repositories
    ├── bin
    └── obj
```

PolicyManager is the main function application. PolicyManager.Client is a client side SDK to talk to the function application. PolicyManager.DataAccess handles the connection between the function application and SQL Azure. PolicyManager.DataAccess.Tests are the unit tests for the data access project.

## REST Calls

I'd recommend testing this API using the integration test called PolicyManagerTests or using [Insomnia REST Client](https://insomnia.rest/) to test the endpoint. Setting up [Insomnia REST Client](https://insomnia.rest/) is simple as explained on this [blog article](https://jwendl.net/2018/11/06/using-insomnia-to-test-aad-v2/).

Read Things

``` bash
curl --request GET \
  --url http://localhost:7071/api/things \
  --header 'authorization: Bearer ey...'
```

Result

``` json
[]
```

Create Thing

``` bash
curl --request POST \
  --url http://localhost:7071/api/things/ \
  --header 'authorization: Bearer ey...' \
  --header 'content-type: application/json' \
  --data '{
	"name": "Customer Data",
	"identifier": "/api/customer/",
	"description": "Enables access to customer data"
}'
```

Result

``` json
{
  "name": "Customer Data",
  "description": "Enables access to customer data",
  "identifier": "/api/customer/",
  "thingAttributes": [],
  "thingPolicies": [],
  "id": "3c713953-d6b4-4edd-beda-79f2fa894a8c"
}
```

Create ThingAttribute

``` bash
curl --request POST \
  --url http://localhost:7071/api/thingAttributes/ \
  --header 'authorization: Bearer ey...' \
  --header 'content-type: application/json' \
  --data '{
	"thingId": "3c713953-d6b4-4edd-beda-79f2fa894a8c",
	"key": "location",
	"value": "Carnation, WA"
}'
```

Result

``` json
{
  "thingId": "3c713953-d6b4-4edd-beda-79f2fa894a8c",
  "thing": null,
  "key": "location",
  "value": "Carnation, WA",
  "id": "0546213f-9f18-45db-ac8b-d231ff46b65c"
}
```

Create ThingPolicy

``` bash
curl --request POST \
  --url http://localhost:7071/api/thingPolicies/ \
  --header 'authorization: Bearer ey...' \
  --header 'content-type: application/json' \
  --data '{
	"thingId": "3c713953-d6b4-4edd-beda-79f2fa894a8c",
	"name": "Is Justin",
	"description": "Checks if the user is Justin",
	"expression": "userPrincipalName = \"live.com#jwendl@hotmail.com\""
}'
```

Result

``` json
{
  "thingId": "3c713953-d6b4-4edd-beda-79f2fa894a8c",
  "thing": null,
  "name": "Is Justin",
  "description": "Checks if the user is Justin",
  "expression": "userPrincipalName = \"live.com#jwendl@hotmail.com\"",
  "id": "96f7ef63-66ac-4211-870c-25045181142d"
}
```

Check Access

``` bash
curl --request POST \
  --url http://localhost:7071/api/checkAccess/ \
  --header 'authorization: Bearer ey...' \
  --header 'content-type: application/json' \
  --data '{
	"requestIdentifier": "/api/customer/"
}'
```

Result

``` json
[
  {
    "name": "Is Justin",
    "description": "Checks if the user is Justin",
    "result": 1,
    "resultString": "Allow"
  }
]
```

