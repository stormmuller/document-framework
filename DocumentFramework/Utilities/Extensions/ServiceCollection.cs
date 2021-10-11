using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
  internal static class ServiceCollectionExtensions
  {
    internal static IServiceCollection AddByConventionAsSingleton<T>(this IServiceCollection services)
    {
      var classes = Assembly.GetCallingAssembly()
        .ExportedTypes
        .Where(type => type.IsClass);

      foreach (var implementation in classes)
      {
        var contract = implementation.GetInterfaces()
          .SingleOrDefault(i => i == typeof(T));

        if (contract != null)
        {
          services.AddSingleton(contract, implementation);
        }
      }

      return services;
    }
  }
}