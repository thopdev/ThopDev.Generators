![Banner](https://user-images.githubusercontent.com/9268249/175341782-661bd327-cba6-415c-8ea7-1a01c5c953a7.png)

# ThopDev.SourceGenerator.Blazor.Routes

This package uses source generation to create strongly typed methods to generate your routes. You simply define your routes and the function to generate your string is generated!

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ThopDev.Generator.Blazor.Routes)](https://www.nuget.org/packages/ThopDev.Generator.Blazor.Routes/)

# Warning!

This package is still in beta!
_I'm working hard to improve this package but it could still contains bugs. If you find any please contact me, so we can fix
it!_

# Usage
Install the package
```csharp
dotnet add package ThopDev.Generator.Blazor.Routes
```
Add using to your class
````csharp
@using ThopDev.Generator.Blazor.Routes
````

To start, define the routes on the code behind component. Currently no support for .razor pages.
Example:

```csharp
[Route("/users/{id:int}")]
[Route("/users/{id:int}/name/{name}")]
public class UserPage {
    [SupplyParameterFromQuery] public string SearchString { get; set; }
    [SupplyParameterFromQuery(Name = "AnotherName")] public int Age { get; set; }
}
```

This will provide you with the following function on the NavigationFactory

```csharp
INavigationFactory factory = new NavigationFactory();

// /users/5
string user = factory.Users().WithId(5).ToRoute();

// /users/12/name/test?SearchString=test&anotherName=42
string userWithName = factory.Users().WithId(12).Name().WithName("test").ToRoute(searchString: "Hello", anotherName: 42);
```
# Examples
| Route                       | Code                                                          | Generates           | 
|-----------------------------|---------------------------------------------------------------|---------------------|
| /                           | factory.ToRoute();                                            | /                   |
| /users                      | factory.Users().ToRoute();                                    | /users              |
| /users/{id:int}             | factory.Users().WithId(5).ToRoute();                          | /users/5            |
| /users/{id:int}/name/{name} | factory.Users().WithId(12).Name().WithName("test").ToRoute(); | /users/12/name/test |
| /groups/roles/name          | factory.Groups().Roles().Name().ToRoute();                    | /groups/roles/name  |
| /groups/foo                 | factory.Groups().Foo().ToRoute();                             | /groups/foo         |

# Limitations
In .Net 6 blazor uses source generation to code compile the .razor pages. This makes it harder to analyze it, for this
reason, we can't create the code for those yet.
To bypass this you can add the following code to your csproj:

```csharp

<PropertyGroup>
  <UseRazorSourceGenerator>false</UseRazorSourceGenerator>
</PropertyGroup> 
```

# Feedback and suggestions  
Any suggestions, feedback or bugs reports are appreciated and can be done though: [feedback](https://github.com/thopdev/ThopDev.Generators/issues/new/choose)
