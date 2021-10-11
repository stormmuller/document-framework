using DocumentFramework;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class MongoFrameworkServiceCollectionExtensions
  {
    public static IServiceCollection AddMongoContext<TMongoContext>([NotNullAttribute] this IServiceCollection serviceCollection) where TMongoContext : MongoContext
    {
      serviceCollection.AddByConventionAsSingleton<IMongoMigration>();
      serviceCollection.AddTransient<TMongoContext>();

      return serviceCollection;
    }
  }
}