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
			Application.Run(args.Length > 0 ? (args[0].ToLower().EndsWith(".wad") ? new Main(args[0]) : args[0].ToLower().EndsWith(".vpk") ? new FileBrowser(args[0]) : new FileBrowser()) : new FileBrowser());
		}
	}
}