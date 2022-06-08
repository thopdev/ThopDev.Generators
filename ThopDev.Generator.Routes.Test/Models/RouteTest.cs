using ThopDev.Generator.Routes.ClassConverters;
using ThopDev.Generator.Routes.Factories;
using ThopDev.Generator.Routes.Models;

namespace ThopDev.Generator.Routes.Test.Models;
[UsesVerify]
public class RouteTest
{
    [Fact]
    public Task Test()
    {
        var component = new Component();
        var routeFactory = new RouteFactory(new RouteSegmentFactory());
        
        var result = ClassConverter.CreateFactoryClassString(new[]
        {
            routeFactory.Create("/test/{id:int}", component),
            routeFactory.Create("/test/{id:int}/bla/i/{what?}", component)
        });

        return Verify(result);
    }
}