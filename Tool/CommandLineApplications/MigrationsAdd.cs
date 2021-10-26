using System;
using System.IO;
using HandlebarsDotNet;
using McMaster.Extensions.CommandLineUtils;

namespace DocumentFramework.Tool.CommandLineApplications
{
    public static class MigrationsAdd
    {
        private const string defaultMigrationDirectory = "Data/Migrations";

        public static CommandLineApplication BuildApplication() 
        {
            var templatesDir = AppDomain.CurrentDomain.BaseDirectory;

            var app = new CommandLineApplication 
            { 
                Name = "add", 
                Description = "Adds a new migration."
            };

            var migrationName = app.Argument("name", "The name of the new migration");
            migrationName.IsRequired();
            
            var migrationDirectory = app.Option("-d|--dir", "The directory to put the migration file in. Paths are relative to the execution directory.", CommandOptionType.SingleValue);
            var migrationNamespace = app.Option("-n|--namespace", "The namespace to use. Matches the directory by default.", CommandOptionType.SingleValue);

            app.OnExecute(() => {
                var contextPath = Path.Combine(
                    Environment.CurrentDirectory, 
                    migrationDirectory.Value() ?? defaultMigrationDirectory);

                var currentFolderName = new DirectoryInfo(Environment.CurrentDirectory).Name;

                var migrationTemplate = File.ReadAllText(Path.Combine(templatesDir, "Templates/Migration.handlebars"));
                var generateMigrationTemplate = Handlebars.Compile(migrationTemplate);

                var timestampedMigrationName = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}_{migrationName.Value}";

                var data = new
                {
                    namespaceName = migrationNamespace.Value() ?? $"{currentFolderName}.{defaultMigrationDirectory.Replace('/', '.')}",
                    className = migrationName.Value,
                    migrationName = timestampedMigrationName
                };

                var hydratedMigrationTemplate = generateMigrationTemplate(data);

                Directory.CreateDirectory(contextPath);

                File.WriteAllText(Path.Combine(contextPath, timestampedMigrationName + ".cs"), hydratedMigrationTemplate);
            });

            return app;
        }
    }
}