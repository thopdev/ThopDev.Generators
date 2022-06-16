using System.Runtime.CompilerServices;

namespace ThopDev.Generator.Routes.Test;

public class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifierSettings.ScrubInlineGuids();
        VerifierSettings.ScrubEmptyLines();
    }
}