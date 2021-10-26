using McMaster.Extensions.CommandLineUtils;

namespace DocumentFramework.Tool.CommandLineApplications
{
    public static class MongoContext
    {
        public static CommandLineApplication BuildApplication() 
        {
            var app = new CommandLineApplication { Name = "dbcontext" };

            app.AddSubcommand(MongoContextScaffold.BuildApplication());

            return app;
        }
    }
}