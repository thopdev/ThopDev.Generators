---
# ThopDev.SourceGenerator.Blazor.Routes
This package uses source generation to create strongly typed methods to generate your routes.   

# Usage
To start define the routes on the code behind component. Currently no support for .razor page.
Example:
```
[Route("/users/{id:int}")]
[Route("/users/{id:int}/name/{name}")]
public class UserPage {}
```
This will provide you with the following function on the NavigationFactory 
```
var factory = new NavigationFactory();

// /users/5
var user = factory.Users().WithId(5).ToRoute();

// /users/12/name/test
var userWithName = factory.Users().WithId(12).Name().WithName("test").ToRoute();
```

# Limitations
In .Net 6 blazor uses source generation to code compile the .razor pages. This makes it harder to analyzing it, for this reason we can't create the code for those yet.
To bypass this you can add the following code to your csproj:
```
<PropertyGroup>
  <UseRazorSourceGenerator>false</UseRazorSourceGenerator>
</PropertyGroup> 
```