
using Microsoft.AspNetCore.Components;
using ThopDev.Generator.Blazor.Routes;

namespace thopDev.Generator.Routes.Examples.Pages;

[Route("/users/{id:int}")]
[Route("/users/{id:int}/name/{name}")]
public partial class Counter
{
    [Parameter] public string? Name { get; set; }
    [SupplyParameterFromQuery] public int Number { get; set; }
    [SupplyParameterFromQuery(Name = "Test")] public int Age { get; set; }
    
    protected override void OnInitialized()
    {
        var factory = new NavigationFactory();
        // /users/5
        var user = factory.Users().WithId(5).ToRoute();
        
        // /users/12/name/test
        var userWithName = factory.Users().WithId(12).Name().WithName("test").ToRoute();

        var c = System.Globalization.CultureInfo.InvariantCulture;
    }
}