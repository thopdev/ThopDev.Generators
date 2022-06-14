using System.Globalization;
using ThopDev.Generator.Blazor.Routes.Models.Routing;

namespace ThopDev.Generator.Blazor.Routes.Examples.Test;

public class NavigationFactoryTest
{
    [Fact]
    public void UsersWithIdNameWithName()
    {
        var navigationFactory = new NavigationFactory();

        var result = navigationFactory.Users().WithId(5).Name().WithName("piet").ToRoute();

        Assert.Equal("/users/5/name/piet", result);
    }    
    
    [Fact]
    public void UsersWithIdNameWithNameWithQueryParameter()
    {
        var navigationFactory = new NavigationFactory();

        var result = navigationFactory.Users().WithId(5).Name().WithName("piet").ToRoute(number: 5, test: 2);

        Assert.Equal("/users/5/name/piet?Number=5&Test=2", result);
    }

    [Fact]
    public void UsersWithId()
    {
        var navigationFactory = new NavigationFactory();

        var result = navigationFactory.Users().WithId(2).ToRoute();

        Assert.Equal("/users/2", result);
    }
}