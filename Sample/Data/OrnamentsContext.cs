using System.Collections.Generic;
using DocumentFramework;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ornaments.Data
{
    public class OrnamentsContext : MongoContext
    {
        public readonly IMongoCollection<SnowGlobe> SnowGlobes;

        public OrnamentsContext(IEnumerable<IMongoMigration> migrations, IMongoDatabase database, ILogger<MongoContext> logger)
            : base(migrations, database, logger)
        {
            SnowGlobes = GetOrAddCollection<SnowGlobe>("snow-globes");
        }
    }
}