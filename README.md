# Sello

[![Build status](https://ci.appveyor.com/api/projects/status/7s8flv5ebqh6j3vv?svg=true)](https://ci.appveyor.com/project/tomkerkhove/sello-bmhc3) [![License](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/tomkerkhove/sello/blob/master/LICENSE)

Sello is a fictious company that is running a multi-tenant SaaS platform for selling products.

Everything is deployed automatically on a per-tenant level and is backed by Visual Studio Team Services Release Management

![Scenario](./docs/scenario.png)

## Simulating failures
For the sake of the demo you can simulate API failures by unleashing the chaos monkeys.

This can be achieved via:
- Configuring the `Demo.UnleashChaosMonkey` application setting to `true` on the API
- Sending the `X-Inject-Chaos-Monkey` custom header with a bogus value

This will result in operations throwing exceptions and the health endpoint to fail

## API Overview
Sello exposes all their APIs via API Management in order to decouple the physical API from the endpoints that their customers are using.

This also enables them to only expose the APIs that 3rd parties need and keep the management APIs internally.

In Azure API Management we have the following setup:

|                             |**3rd Party**                     |**Management**                                          |**Operations**                             |
|:----------------------------|:--------------------------------:|:------------------------------------------------------:|------------------------------------------:|
|Product Name                 |Sello (Free & Premium)            |Sello - Management                                      |Sello - Operations                         |
|Published in Developer Portal|:white_check_mark:                |:x:                                                     |:x:                                        |
|Subscription Required        |:white_check_mark:                |:white_check_mark:                                      |:white_check_mark:                         |
|Subscription Approval        |:white_check_mark:                |:white_check_mark:                                      |:x:                                        |
|Throttling                   |:white_check_mark:, Product-level |:x:                                                     |:white_check_mark:, only on health-endpoint|
|API(s)                       |<ul><li>Sello API</li></ul>       |<ul><li>Sello API</li><li>Sello Management API</li></ul>|<ul><li>Sello Operations API</li></ul>     |

These will communicate with the physical API that is hosted in an Azure Web App.

|:rotating_light: **Security**                                                                        |
|-----------------------------------------------------------------------------------------------------|
| For the sake of the demo there are some gaps in API security:<ul><li>Physical API has no authentication and authorization</li><li>Physical API is publically reachable</li><li>No security between Azure API Management & the physical API</li></ul>This is not safe for production workloads and thus not recommended.|

### Automating Azure API Management
We are currently automatically importing the Swagger specification for both the public & management API.

This can be achieved as following:
```PowerShell
Import-AzureRmApiManagementSwaggerDefinition.ps1 -apiManagementInstanceName "<instance-name>" -resourceGroupName "<resource-group-name>" -swaggerDefinitionPath "<swagger-definition-path>" -apiId "<api-management-api-id>" -apiUrlSuffix "<logical-api-suffix>" -apiUrl "<url-physical-api>" -apiDefaultName "<default-api-name-in-swagger-definition>" -apiName "<desired-logical-api-name>"
```

Policies can be applied to both products and operations.
Here is how you automatically apply policies on a product-level:
```PowerShell
Set-AzureRmApiManagementPolicy.ps1 -apiManagementInstanceName "<instance-name>" -resourceGroupName "<resource-group-name>" -policyDefinitionPath "<policy-definition-path>" -policyType "product" -productId "<api-management-product-id>"
```

## License Information
This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Codit is the original author of this web application.