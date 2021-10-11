using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using DocumentFramework.Models;

namespace DocumentFramework
{
  public abstract class MongoContext
  {
    protected const string MIGRATIONS_COLLECTION_NAME = "_migrations";

    protected readonly IMongoDatabase _database;
    private readonly IEnumerable<IMongoMigration> _migrations;
    private readonly ILogger<MongoContext> _logger;

    protected MongoContext(
      IEnumerable<IMongoMigration> migrations,
      IMongoDatabase database,
      ILogger<MongoContext> logger
    )
    {
      _migrations = migrations;
      _database = database;
      _logger = logger;
    }

    protected IMongoCollection<TModel> GetOrAddCollection<TModel>(string collectionName)
    {
      if (!CollectionExists(collectionName))
      {
        _logger.LogInformation("Collection '{collectionName}' does not exist, going to attempt to create the collection", collectionName);
        _database.CreateCollection(collectionName);
        _logger.LogInformation("Created {collectionName} collection", collectionName);
      }

      return _database.GetCollection<TModel>(collectionName);
    }

    private bool CollectionExists(string collectionName)
    {
      var filter = new BsonDocument("name", collectionName);
      var options = new ListCollectionNamesOptions { Filter = filter };

      return _database.ListCollectionNames(options).Any();
    }

    private IEnumerable<Migration> GetMigrationsAlreadyRun()
    {
      _logger.LogInformation("Finding all migrations already run");

      var migrationsCollection = GetOrAddCollection<Migration>(MIGRATIONS_COLLECTION_NAME);

      var migrationsAlreadyRun = migrationsCollection
        .Find(m => true) // Get all the documents
        .ToEnumerable()
        .OrderBy(m => m.Date);

      _logger.LogInformation("Found {numberOfMigrationsRun} migrations that have already been run", migrationsAlreadyRun.Count());

      foreach (var migration in migrationsAlreadyRun)
      {
        _logger.LogInformation("{migrationName} was already run at {timeMigrationWasRun}", migration.Name, migration.Date);
      }

      return migrationsAlreadyRun;
    }

    private void WriteToMigrationsCollection(IMongoMigration migration)
    {
      _logger.LogInformation("Updating migrations collection to include {migrationName}", migration.MigrationName);
      var migrationsCollection = GetOrAddCollection<Migration>(MIGRATIONS_COLLECTION_NAME);

      migrationsCollection.InsertOne(new Migration { Name = migration.MigrationName, Date = DateTime.UtcNow });
    }

    public void SyncMigrations()
    {
      _logger.LogInformation("Syncing migrations...");

      var migrationsToIgnore = GetMigrationsAlreadyRun();

      foreach (var migration in _migrations)
      {
        var migrationHasRun = migrationsToIgnore.Any(ignoredMigration => ignoredMigration.Name == migration.MigrationName);

        if (!migrationHasRun)
        {
          _logger.LogInformation("Running migration {migrationName}", migration.MigrationName);
          migration.MigrateForward();
          WriteToMigrationsCollection(migration);
        }
      }

      _logger.LogInformation("Done Syncing migrations!");
    }
  }
}