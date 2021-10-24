namespace DocumentFramework.Tool
{
    internal interface ICommand
    {
        public string Text { get; }
        public string ShorthandText { get; }
        public string Description { get; }

        public void HandleCommand(string[] args);
    }
}