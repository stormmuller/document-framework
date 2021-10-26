using System;
using System.Collections.Generic;
using DocumentFramework;
using MongoDB.Driver;
using Ornaments;

namespace Sample.Data.Migrations
{
    public class SeedSnowGlobes : IMongoMigration
    {
        public string MigrationName => "20211026112147_SeedSnowGlobes";

        private readonly IMongoDatabase _database;

        public SeedSnowGlobes(IMongoDatabase database)
        {
            _database = database;
        }

        public void MigrateForward()
        {
            var snowGlobeCollection = _database.GetCollection<SnowGlobe>("snow-globes");

            var snowGlobes = new List<SnowGlobe>
                {
                    new SnowGlobe { Name = "Winter Wonder", Description = "cottage covered in snow" },
                    new SnowGlobe { Name = "Winter Joy", Description = "snowman smiling and holding a large muffin" },
                    new SnowGlobe { Name = "Brightness", Description = "shooting star" }
                };

            snowGlobeCollection.InsertMany(snowGlobes);
        }

        public void MigrationBackward()
        {
            throw new NotImplementedException();
        }
    }
}