using System.Reflection;

namespace FooCommerce.Domain.Helpers;

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
            .Distinct()
            .Select(AssemblyName.GetAssemblyName)
            .Where(assemblyName => assemblyName.FullName.StartsWith(nameof(FooCommerce)) && !assemblyName.FullName.EndsWith(".Tests"))
            .Select(Assembly.Load);
        return assemblies;
    }
}