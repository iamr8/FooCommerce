using System.Reflection;

namespace FooCommerce.Application.Helpers;

public static class AssemblyHelper
{
    public static IEnumerable<Assembly> GetExecutingAssemblies(this AppDomain domain)
    {
        var assemblies = domain.GetAssemblies()
            .Where(assembly => assembly.GetName().Name.StartsWith(nameof(FooCommerce)))
            .Where(assembly => !assembly.GetName().Name.EndsWith(".Tests"));
        return assemblies;
    }

    public static IEnumerable<Assembly> GetSolutionAssemblies(this AppDomain domain)
    {
        var assemblies = Directory.EnumerateFiles(domain.BaseDirectory)
            .Where(assemblyFile => Path.GetExtension(assemblyFile).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
            .Select(Path.GetFileNameWithoutExtension)
            .Where(assemblyFile => assemblyFile.StartsWith(nameof(FooCommerce)))
            .Where(assemblyFile => !assemblyFile.EndsWith(".Tests"))
            .Select(assemblyFile => Assembly.LoadFile(Path.Combine(domain.BaseDirectory, $"{assemblyFile}.dll")));
        return assemblies;
    }
}