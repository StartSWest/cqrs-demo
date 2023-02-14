# **Install Instructions**

**Wellcome to the Vueling.Otd.Backend project**

The project uses Dapr and Docker to enable distribute services with a redis cache. Please follow the steps below to setup your enviroment.

1. Install Dapr cli ([https://docs.dapr.io/getting-started/install-dapr-cli/](https://docs.dapr.io/getting-started/install-dapr-cli/))

2. Install Docker ([https://docs.docker.com/install/](https://docs.docker.com/install/))

3. Initialize Dapr (it should download the containers and create a default config)
```
> dapr init
```
4. Review configuration ([https://docs.dapr.io/operations/configuration/configuration-overview/](https://docs.dapr.io/operations/configuration/configuration-overview/))
  
  On windows: *%USERPROFILE%\\.dapr*
  
**Please Note:**

- redis configuration at *%USERPROFILE%\\.dapr\components\statestore.yaml*
- The default name for the store is *statestore* but it can be changed on *statestore.yaml* and *appsettings.json* on *Vueling.Otd.WebService* project.
  
5. Run Dapr sidecar with redis and *Vueling.Otd.WebService*

- Clone the git project, stand at root and run:
```
> dapr run --app-id vueling --dapr-http-port 3500 -- dotnet run --project .\Vueling.Otd.WebService\Vueling.Otd.
WebService.csproj
```

6. Open browser at *http://localhost:5204/swagger/index.html*

7. To debug the app in Visual Studio attach the debugger to the *Vueling.Otd.WebService* process.

8. Configuration *Vueling.Otd.WebService/appsettings.json*
```
  "GatewayApi": {
    "BaseUrl": "http://quiet-stone-2094.herokuapp.com",
    "RatesFilename": "rates.json",
    "TransactionsFilename": "transactions.json"
  },

  "StateStore": {
    "StoreName": "statestore"
  }
```

9. Testing the cache is really easy, just put a wrong *"BaseUrl"* in "GatewayApi". The request will fail and the data will be taken from the redis cache if it was successfully saved by success on at least one request.

10. Running Tests
```
dapr run --app-id vueling --dapr-http-port 3500 -- dotnet test
```

# **Some Words**

The project has a Clean Architecture of four layers *Domain*, *Application*, *Infrastructure* and *WebService*. It uses a strong understanding of SOLID principles and Inversion of Control along with a few other practices such as: CQRS and Mediator Patterns, that allow great decoupling throughout the solution. It also uses a global exception logger with some specific exception handling patterns for the required cases.

The solution contains both unit tests and integration tests for the domain and application layer. More details below.

**Domain layer**

It contains mostly low-level business logic, such as: Dijkstra's shortest path algorithm to find the missing currency pairs using the given information, and some value objects to encapsulate the rounding strategy required for rates. It divides the responsibilities between two large groups for rates and transactions. This layer ensures no external dependencies and can be fully reusable. Can be fully unit tested.

**Application layer**

It represents the top layer of the business. It defines the public interfaces and DTOs to communicate with the outer layers and defines the specific enterprise level use cases and logic via the CQRS pattern without knowing anything about the underlying platform also exposes and throws custom application exceptions. Enables integration testing capabilities.

**Infrastructure layer**

This layer implements some of the *Application layer* interfaces for getting the required data from the external Gateway API and to persist data into a redis cache.

**WebService layer**

Higher level service layer that only exposes the API http controller for rates and transactions and uses the *Mediator pattern* to communicate with the *Application layer* to get the necessary data. It is also where some of the web service configuration is located.