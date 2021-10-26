using McMaster.Extensions.CommandLineUtils;

namespace DocumentFramework.Tool.CommandLineApplications
{
    public static class Migrations
    {
        public static CommandLineApplication BuildApplication() 
        {
            var app = new CommandLineApplication { Name = "migrations" };
            
            app.AddSubcommand(MigrationsAdd.BuildApplication());

            return app;
        }
    }
}