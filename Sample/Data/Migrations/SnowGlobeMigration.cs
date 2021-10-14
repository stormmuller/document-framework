using DocumentFramework;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Ornaments
{
    public class SnowGlobeMigration : IMongoMigration
    {
        public string MigrationName => "First_Snow_Globe_Migration";

        private readonly IMongoDatabase _database;

        public SnowGlobeMigration(IMongoDatabase database)
        {
            _database = database;
        }

        public void MigrateForward()
        {
            var snowGlobeCollection = _database.GetCollection<SnowGlobe>("snowGlobes");

            var snowGlobes = new List<SnowGlobe> {
            new SnowGlobe { Name = "White Winter", Description = "cottage covered in snow" },
            new SnowGlobe { Name = "White Joy", Description = "snowman smiling and holding a large muffin" },
            new SnowGlobe { Name = "Brightness", Description = "shooting star" }
        };

            snowGlobeCollection.InsertMany(snowGlobes);
        }
    }
}