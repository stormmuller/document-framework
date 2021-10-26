using McMaster.Extensions.CommandLineUtils;

namespace DocumentFramework.Tool.CommandLineApplications
{
    public static class Root
    {
        public static CommandLineApplication BuildApplication() 
        {
            var app = new CommandLineApplication();
            
            app.HelpOption("-h|--help", true);
            app.AddSubcommand(Migrations.BuildApplication());
            app.AddSubcommand(MongoContext.BuildApplication());

            return app;
        }
    }
}