# Hot Recharge API SDK
## Version 3 API 
Hot Recharge is a Zimbabwean recharge platform for all networks and various utility services. 
The package allows for users to easily add the version 3.0 API to your c# applications.  

## Usage - With FactoryClient 
1) Add Package to Project 
2) Create client with credentials
```
 IHotAPIClient client = HotApiClientFactory.Create("{username}", "{password}");  
```

## Usage - As Injected Service
1) Add Package to project
2) Register the SDK in service for DI
```
var Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName ?? "")
             .AddJsonFile("appsettings.json", false)
             .AddUserSecrets(Assembly.GetExecutingAssembly())
             .AddEnvironmentVariables()
             .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {  
        services.AddHotApiSdk(Configuration);
    })
    .Build();
```
3) Add credentials to configuration file 
```
 "HotAPI": {
    "Username": "{AccessCode}",
    "Password": "{Password}"
  }
```
4) Inject Service into application in constructor
```
    public class App { 
        private readonly IHotAPIClient _client;
        public App (IHotAPIClient client) {
            _client = client;
        }
        ...
    }
```
## Example Usage
```
    // Account Balance Query
    var response = await client.Account.BalancesAsync(); 
    if (response.IsSuccessStatusCode)
    {
        result = JsonSerializer.Serialize(response.Content);
    }
    else
    {
        result = JsonSerializer.Serialize(response.Error.Content);
    }
    Console.WriteLine(result);
```

## Implement it yourself 
An API guide for Hot Recharge API is available [here](https://hot-api-v3.readme.io/)
 
## Features
- Recharge Airtime for all mobile networks operators - Econet, Netone, Telecel
- ZETDC Token Purchases
- Utility Payments for Telone & Nyaradzo
- Custom SMS notification message for clients
- Easily query information for bundles & products available

## License

MIT
