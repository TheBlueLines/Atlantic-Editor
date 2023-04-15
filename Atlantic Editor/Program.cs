namespace Atlantic_Editor
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Main main = new Main(args.Length > 0 ? args[0] : null);
            Application.Run(main);
        }
    }
}