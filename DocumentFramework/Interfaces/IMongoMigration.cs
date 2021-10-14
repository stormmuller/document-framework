namespace DocumentFramework
{
  public interface IMongoMigration
  {
    string MigrationName { get; }
    void MigrateForward();
    void MigrationBackward();
  }
}