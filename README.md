# ACGSS
Sample Web App implementation with .NET Core and DDD using Clean Architecture.

## Solution Design
The solution design focuses on a basic Domain Driven Design techniques and implementation, while keeping the things as simple as possible but can be extended as needed. Multiple assemblies are used for separation of concerns to keep logic isolated from the other components. **.NET 10 C#** is the default framework and language for this application.

### Assembly Layers
-   **ACGSS.Domain**  - This assembly contains common, entities and interfaces.
-   **ACGSS.Application**  - This assembly contains all services implementations.
-   **ACGSS.Infrastructure**  - This assembly contains the infrastructure of data persistence.
-   **ACGSS.Web**  - This assembly is the web app host.
-   **ACGSS.Tests**  - This assembly contains unit test classes based on the [NUnit](https://github.com/nunit/nunit) testing framework.

## Validation
Data validation using [FluentValidation](https://github.com/JeremySkinner/FluentValidation)

## How to run application: 

1. Create empty Database, name: **`ACGSS`**
2. Set Connection String (in [appsettings.json](https://github.com/Jadhielv/ACGSS/blob/main/ACGSS.Web/appsettings.json#L9) or by user secrets mechanism)
3. Execute [migrations](https://github.com/Jadhielv/ACGSS/tree/main/ACGSS.Infrastructure/Migrations).
4. Set Authentication | Twilio - SendGrid (in [appsettings.json](https://github.com/Jadhielv/ACGSS/blob/main/ACGSS.Web/appsettings.json#L12))
5. Run .. .

## ¡Give a Star!

If you like this project, learn something or you are using it in your applications, please give it a star. Thanks! .. .

## License

This project is open source and available under the -> [MIT License](LICENSE)
