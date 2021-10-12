[![.NET](https://github.com/stormmuller/document-framework/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/stormmuller/document-framework/actions/workflows/dotnet.yml)

# Document Framework

A light weight ORM for moongo and dotnet. 

## Notable features

 - EF style registration and usage
 - Migrations

## Usage

1. Add a model
``` csharp
public class Foo
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
  public string Text { get; set; }
}
```

2. Create a `MongoContext`
``` csharp
public class BarMongoContext : MongoContext
{
  public readonly IMongoCollection<Foo> Foos;

  public BarMongoContext(IMongoDatabase database, IEnumerable<IMongoMigration> migrations, ILogger<MongoContext> logger)
    : base(migrations, database, logger)
  {
    Foos = GetOrAddCollection<Foo>("foos");
  }
}
```

3. Create a migration 

``` csharp
public class SeedFoos : IMongoMigration
{
  public string MigrationName => "20210420000000_Foos";
  private readonly IMongoDatabase _database;

  public SeedFoos(IMongoDatabase database)
  {
    _database = database;
  }

  public void MigrateForward()
  {
    var fooCollection = _database.GetCollection<Foo>("foos");

    fooCollection.InsertMany(new Foo[] { 
        new Foo { Text = "document-1" }, 
        new Foo { Text = "document-2" }
    });
  }
}
```

4. Register Context and mongo database in the `ConfigureServices` method in your `Startup.cs`

``` csharp
services
    .AddMongoContext<BarMongoContext>()
    .AddTransient<IMongoDatabase>()
```

5. Sync migrations in the `Configure` method in your `Startup.cs`

``` csharp
app.ApplicationServices
        .GetRequiredService<BarMongoContext>()
        .SyncMigrations();
```

6. Use the Mongo context to query collections

``` csharp
public class SomeService
{
    private readonly BarMongoContext _context;

    public SomeService(BarMongoContext context)
    {
        _context = context;
    }

    public IEnumerable<Foo> GetSomeFooStuff(string text) 
    {
        _context.Foos.Find(f => f.Text == text).ToEnumerable();
    }
}
```