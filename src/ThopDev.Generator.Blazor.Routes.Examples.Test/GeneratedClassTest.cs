using ThopDev.Generator.Blazor.Routes;

namespace ThopDev.Generator.Blazor.Routes.Examples.Test;

public class GeneratedClassTest
{
    [Theory]
    [InlineData(typeof(NavigationFactory), typeof(INavigationFactory))]
    public void IsClassGeneratedWithInterface(Type classType, Type interfaceType)
    {
        Assert.Contains(interfaceType, classType.GetInterfaces());
    }    
    
    [Theory]
    [InlineData(typeof(UsersWithIdNameWithNameRoute), typeof(RoutableBase))]
    [InlineData(typeof(UsersWithIdRoute), typeof(RoutableBase))]
    [InlineData(typeof(UsersWithIdNameRoute), typeof(RoutingBase))]
    [InlineData(typeof(UsersRoute), typeof(RoutingBase))]
    public void IsClassGeneratedWithBaseType(Type classType, Type baseType)
    {
        Assert.Equal(classType.BaseType, baseType);
    }
}