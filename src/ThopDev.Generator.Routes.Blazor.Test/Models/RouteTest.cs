using ThopDev.Generator.Blazor.Routes.ClassConverters;
using ThopDev.Generator.Blazor.Routes.Factories;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Routes.Test.Models;

[UsesVerify]
public class RouteTest
{
    [Fact]
    public Task ValidateGenerateFile()
    {
        var component = new ComponentModel()
        {
            Name = "TestComponent",
            Namespace = "ThopDev.Test",
            QueryParameters = new List<QueryParameter>{new() {Name = "FirstName", Type = "string"}, new() {Name = "Age", Type = "int"}}
        };
        var routeFactory = new RouteFactory(new RouteSegmentFactory());
        var routeGroupingFactory = new RouteGroupingFactory();

        var routes = new[]
        {
            routeFactory.Create("/test/{id:int}", component),
            routeFactory.Create("/test/{id:int}/bla/i/{what?}", component)
        };

        var allGroupRoutes = routeGroupingFactory.GetAllGroupRoutes(routes);


        var result = ClassConverter.CreateFactoryClassString(allGroupRoutes);

        return Verify(result);
    }
}