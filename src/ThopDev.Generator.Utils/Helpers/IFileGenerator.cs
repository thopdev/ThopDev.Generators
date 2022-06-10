using System;

namespace ThopDev.Generator.Utils.Helpers;

public interface IFileGenerator : IDisposable
{
    public IFileGenerator AddUsing(string value);
    public INamespaceGenerator OpenNamespace(string value);
}