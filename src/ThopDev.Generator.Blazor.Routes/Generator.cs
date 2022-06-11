using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ThopDev.Generator.Blazor.Routes.ClassConverters;
using ThopDev.Generator.Blazor.Routes.Constants;
using ThopDev.Generator.Blazor.Routes.Factories;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Blazor.Routes;

[Generator]
public class Generator : ISourceGenerator
{
    private readonly RouteFactory _routeFactory;
    private readonly RouteGroupingFactory _routeGroupingFactory;

    public Generator()
    {
        _routeGroupingFactory = new RouteGroupingFactory();
        _routeFactory = new RouteFactory(new RouteSegmentFactory());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new WrapperAttributeSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxReceiver as WrapperAttributeSyntaxReceiver;
        var compilation = context.Compilation;

        var routes = GetClassRoutes(receiver, compilation).ToList();
        var groups = _routeGroupingFactory.GetAllGroupRoutes(routes);
        // ReSharper disable once PossibleNullReferenceException
        var textWriter = ClassConverter.CreateFactoryClassString(groups);
        context.AddSource("NavigationFactory.g.cs", textWriter);
        context.AddSource("RoutingBase.g.cs", StaticFiles.RoutingBase);

    }


    private IEnumerable<RouteModel> GetClassRoutes(WrapperAttributeSyntaxReceiver receiver, Compilation compilation)
    {
        foreach (var routeClass in receiver.ClassToAugments)
        {
            var component = new ComponentModel { Name = routeClass.Identifier.Text };
            foreach (var route in GetRoutes(compilation, routeClass))
                yield return _routeFactory.Create(route, component);
        }
    }

    private static IEnumerable<string> GetRoutes(Compilation compilation, ClassDeclarationSyntax routeClass)
    {
        var semanticModel = compilation.GetSemanticModel(routeClass.SyntaxTree);
        foreach (var routeAttribute in routeClass.AttributeLists.SelectMany(attr =>
                     attr.Attributes.Where(x => x.Name.ToString() == "Route")))
        {
            var routeArg = routeAttribute?.ArgumentList?.Arguments[0];
            var routeExpr = routeArg?.Expression;
            if (routeExpr != null) yield return semanticModel.GetConstantValue(routeExpr).ToString();
        }
    }
}