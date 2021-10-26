using System;
using System.IO;
using HandlebarsDotNet;
using McMaster.Extensions.CommandLineUtils;

namespace DocumentFramework.Tool.CommandLineApplications
{
    public static class MongoContextScaffold
    {
        private const string defaultContextDirectory = "Data";

        public static CommandLineApplication BuildApplication()
        {
            var templatesDir = AppDomain.CurrentDomain.BaseDirectory;

            var app = new CommandLineApplication
            {
                Name = "scaffold",
                Description = "Scaffolds a MongoContext and document types for a database."
            };

            var contextName = app.Option("-c|--context", "The name of the document context", CommandOptionType.SingleValue);
            contextName.IsRequired();

            var contextDirectory = app.Option("-d|--dir", "The directory to put the context file in. Paths are relative to the execution directory.", CommandOptionType.SingleValue);
            var contextNamespace = app.Option("-n|--namespace", "The namespace to use. Matches the directory by default.", CommandOptionType.SingleValue);


            app.OnExecute(() =>
            {
                var contextPath = Path.Combine(
                    Environment.CurrentDirectory, 
                    contextDirectory.Value() ?? defaultContextDirectory);

                var currentFolderName = new DirectoryInfo(Environment.CurrentDirectory).Name;

                var mongoContextTemplate = File.ReadAllText(Path.Combine(templatesDir, "Templates/MongoContext.handlebars"));
                var generateMongoContextTemplate = Handlebars.Compile(mongoContextTemplate);

                var data = new
                {
                    namespaceName = contextNamespace.Value() ?? $"{currentFolderName}.{defaultContextDirectory}",
                    className = contextName.Value()
                };

                var hydratedMongoContextTemplate = generateMongoContextTemplate(data);

                Directory.CreateDirectory(contextPath);

                File.WriteAllText(Path.Combine(contextPath, contextName.Value() + ".cs"), hydratedMongoContextTemplate);
            });

            return app;
        }
    }
}