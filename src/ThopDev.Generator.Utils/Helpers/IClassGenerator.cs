using System;
using Microsoft.CodeAnalysis;

namespace ThopDev.Generator.Utils.Helpers;

public interface IClassGenerator
{
    public IClassGenerator AddConst(Accessibility accessibility, string type, string name, string value);
    
    public IMethodGenerator OpenConstructor(Accessibility accessibility, (string, string)[] parameters = null,
        string[] baseParameters = null);
    
    public IMethodGenerator OpenMethod(Accessibility accessibility, string returnType, string name, (string, string)[] parameters = null);

    public INamespaceGenerator CloseClass();
}