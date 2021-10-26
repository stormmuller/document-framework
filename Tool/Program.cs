using DocumentFramework.Tool.CommandLineApplications;

namespace DocumentFramework.Tool
{
    class Program
    {
        public static int Main(string[] args)
        {
            var app = Root.BuildApplication();

            return app.Execute(args);
        }
    }
}
