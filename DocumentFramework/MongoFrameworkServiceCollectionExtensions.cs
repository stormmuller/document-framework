using DocumentFramework;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class MongoFrameworkServiceCollectionExtensions
  {
    public static IServiceCollection AddMongoContext<TMongoContext>([NotNullAttribute] this IServiceCollection serviceCollection) where TMongoContext : MongoContext
    {
      serviceCollection.AddByConventionAsSingleton<IMongoMigration>(Assembly.GetCallingAssembly());
      serviceCollection.AddTransient<TMongoContext>();

      return serviceCollection;
    }
  }
}