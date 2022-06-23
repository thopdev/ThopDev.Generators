using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ThopDev.Generator.Blazor.Routes.ClassConverters;
using ThopDev.Generator.Blazor.Routes.Constants;
using ThopDev.Generator.Blazor.Routes.Factories;
using ThopDev.Generator.Blazor.Routes.Helpers;
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
        try
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
        catch (Exception e)
        {
            Debugger.Launch();
            throw;
        }
    }


    private IEnumerable<RouteModel> GetClassRoutes(WrapperAttributeSyntaxReceiver receiver, Compilation compilation)
    {
        foreach (var routeClass in receiver.ClassToAugments)
        {
            var semanticModel = compilation.GetSemanticModel(routeClass.SyntaxTree);
            
            var component = new ComponentModel { Name = routeClass.Identifier.Text , QueryParameters = GetQueryParameters(semanticModel, routeClass).ToList(), Namespace = routeClass.GetNamespace()};
            foreach (var route in GetRoutes(semanticModel, routeClass))
                yield return _routeFactory.Create(route, component);
        }
    }

    private IEnumerable<QueryParameter> GetQueryParameters(SemanticModel semanticModel, ClassDeclarationSyntax routeClass)
    {
        var properties = routeClass.Members.Where(m => m.Kind() == SyntaxKind.PropertyDeclaration).Cast<PropertyDeclarationSyntax>();

        foreach (var property in properties)
        {
            var queryRouteAttributes = GetAttributes(property.AttributeLists, "SupplyParameterFromQuery");

            foreach (var attr in queryRouteAttributes)
            {
                var name = GetAttributeParameter(attr, semanticModel) ?? property.Identifier.ValueText;
                var type = (property.Type as PredefinedTypeSyntax)?.Keyword.ValueText ?? "string";
                yield return new QueryParameter{Name = name, Type = type};
            }
        }
    }

    private static IEnumerable<string> GetRoutes(SemanticModel semanticModel, ClassDeclarationSyntax routeClass)
    {
        var attributes = GetAttributes(routeClass.AttributeLists, "Route");
        foreach (var routeAttribute in attributes)
        {
            yield return GetAttributeParameter(routeAttribute, semanticModel);
        }
    }

    private static string GetAttributeParameter(AttributeSyntax routeAttribute, SemanticModel semanticModel)
    {
        var routeArg = routeAttribute?.ArgumentList?.Arguments[0];
        var routeExpr = routeArg?.Expression;
        return routeExpr != null ? semanticModel.GetConstantValue(routeExpr).ToString() : null;
    }

    private static IEnumerable<AttributeSyntax> GetAttributes(SyntaxList<AttributeListSyntax> attributeList, string name)
    {
        return attributeList.SelectMany(attr =>
            attr.Attributes.Where(x => x.Name.ToString() == name));
    }
}
