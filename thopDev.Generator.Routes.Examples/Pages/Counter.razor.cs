using Microsoft.AspNetCore.Components;

namespace thopDev.Generator.Routes.Examples.Pages;

[Route("/test/{id:int}")]
[Route("/test/{id:int}/bla/i/{what?}")]
public partial class Counter
{
    [Parameter] public string? What { get; set; }
}