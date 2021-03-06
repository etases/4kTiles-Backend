# 4kTiles - Backend
## Module Purpose
- `Context`: the DbContext
- `Entities`: the Models for the Database
- `Controllers`: the API controllers, which is the endpoint of the API system
- `Services`: the logic code to interact with `Context` & `Entities`
- `DataObjects`: the objects to transfer between modules
  - `DTO`: Input & Output of `Controllers`, Used to transfer data between `API Endpoint` - `Controllers`
  - `DAO`: Input & Output of `Services`, Used to transfer data between `Controllers` - `Services` & `Services` - `Services`
- `Mappers`: Map properties from one object to another, use to convert `Entities` to `DataObjects` and vice versa
  - https://codelearn.io/sharing/su-dung-automapper-trong-csharp
- `Helpers`: The utility methods to be used in some classes

## Project Graph
![graph](.github/Images/ProjectGraph.png)

## Code Style
https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
