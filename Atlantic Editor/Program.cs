namespace Atlantic_Editor
{
	public static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			ApplicationConfiguration.Initialize();
			Application.Run(args.Length > 0 ? (args[0].ToLower().EndsWith(".wad") ? new TextureBrowser(args[0]) : args[0].ToLower().EndsWith(".vpk") ? new FileBrowser(args[0]) : new FileBrowser()) : new FileBrowser());
		}
	}
}