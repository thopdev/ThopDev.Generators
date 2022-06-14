![Banner](https://user-images.githubusercontent.com/9268249/173248498-e8f8fd50-e14e-4359-a8e0-21ea63df98ba.png)
---

# ThopDev.SourceGenerator.Blazor.Routes

This package uses source generation to create strongly typed methods to generate your routes.

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ThopDev.Generator.Blazor.Routes)](https://www.nuget.org/packages/ThopDev.Generator.Blazor.Routes/)

# Warning!

This package is still in beta!
_I'm working hard to improve this package but it still contain bugs. If you find any please contact me so we can fix
it!_

# Usage

To start define the routes on the code behind component. Currently no support for .razor page.
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
var factory = new NavigationFactory();

// /users/5
var user = factory.Users().WithId(5).ToRoute();

// /users/12/name/test?SearchString=test&anotherName=42
var userWithName = factory.Users().WithId(12).Name().WithName("test").ToRoute(searchString: "Hello", anotherName: 42);
```

# Limitations

In .Net 6 blazor uses source generation to code compile the .razor pages. This makes it harder to analyzing it, for this
reason we can't create the code for those yet.
To bypass this you can add the following code to your csproj:

```csharp
<PropertyGroup>
  <UseRazorSourceGenerator>false</UseRazorSourceGenerator>
</PropertyGroup> 
```
