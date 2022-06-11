using ThopDev.Generator.Blazor.Routes.ClassConverters;
using ThopDev.Generator.Blazor.Routes.Factories;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Routes.Test.Models;

[UsesVerify]
public class RouteTest
{
    [Fact]
    public Task Test()
    {
    
        var component = new ComponentModel();
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