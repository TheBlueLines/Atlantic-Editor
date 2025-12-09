using System.Diagnostics;
using TTMC.BPList;
using TTMC.ORE;
using TTMC.VTF;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Atlantic_Editor
{
	public static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			if (args.Length >= 1 && (args[0].ToLower().Equals("ore") || args[0].ToLower().Equals("deore")))
			{
				if (args.Length >= 2)
				{
					if (args[0].ToLower().Equals("deore"))
					{
						string orePath = args[1];
						string dirPath = Path.GetFileNameWithoutExtension(orePath);
						if (args.Length >= 3)
						{
							dirPath = args[2];
						}
						if (File.Exists(orePath))
						{
							Directory.CreateDirectory(dirPath);
							FileStream fileStream = File.OpenRead(orePath);
							ORE ore = new(fileStream);
							foreach (Dir dir in ore.dirs)
							{
								Directory.CreateDirectory(Path.Combine(dirPath, dir.name));
								foreach (Content content in dir.contents)
								{
									File.WriteAllBytes(Path.Combine(dirPath, dir.name, content.name), content.data);
								}
							}
							foreach (Content content in ore.contents)
							{
								File.WriteAllBytes(Path.Combine(dirPath, content.name), content.data);
							}
							return;
						}
					}
					else if (args[0].ToLower().Equals("ore"))
					{
						string dirPath = args[1];
						string orePath = dirPath + ".ore";
						if (args.Length >= 3)
						{
							orePath = args[2];
						}
						if (Directory.Exists(dirPath))
						{
							ORE ore = new();
							string[] fileNames = Directory.GetFiles(dirPath);
							string[] dirNames = Directory.GetDirectories(dirPath);
							Content[] contents = new Content[fileNames.Length];
							Dir[] dirs = new Dir[dirNames.Length];
							for (int i = 0; i < contents.Length; i++)
							{
								contents[i] = new(Path.GetFileName(fileNames[i]), File.ReadAllBytes(fileNames[i]));
							}
							for (int i = 0; i < dirs.Length; i++)
							{
								string[] dirFileNames = Directory.GetFiles(dirNames[i]);
								Content[] dirContents = new Content[dirFileNames.Length];
								for (int j = 0; j < dirContents.Length; j++)
								{
									dirContents[j] = new(Path.GetFileName(dirFileNames[j]), File.ReadAllBytes(dirFileNames[j]));
								}
								dirs[i] = new(Path.GetFileName(dirNames[i]), dirContents);
							}
							ore.contents = contents;
							ore.dirs = dirs;
							File.WriteAllBytes(orePath, ore.ToByteArray());
							return;
						}
					}
				}
				MessageBox.Show("Usage:\n\nDirectory to ore:\naedit ore <directory> <file>\nOre to directory:\naedit deore <file> <directory>", "Atlantic Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			ApplicationConfiguration.Initialize();
			Application.SetColorMode(SystemColorMode.System);
			if (args.Length > 0)
			{
				if (args[0].ToLower().EndsWith(".wad"))
				{
					Application.Run(new TextureBrowser(args[0]));
				}
				else if (args[0].ToLower().EndsWith(".vpk") || args[0].ToLower().EndsWith(".ore"))
				{
					Application.Run(new FileBrowser(args[0]));
				}
				else if (args[0].ToLower().EndsWith(".vtf"))
				{
					FileStream stream = File.OpenRead(args[0]);
					try
					{
						VTF vtf = new(stream);
						MessageBox.Show(vtf.width + " " + vtf.height);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					//Application.Run(new ImageViewer(stream));
					stream.Close();
					stream.Dispose();
				}
				else if (args[0].ToLower().EndsWith(".plist"))
				{
					FileStream stream = File.OpenRead(args[0]);
					try
					{
						BPList list = new(stream);
						MessageBox.Show("Binary Property List file version: " + list.version);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					//Application.Run(new ImageViewer(stream));
					stream.Close();
					stream.Dispose();
				}
				else
				{
					Application.Run(new FileBrowser());
				}
			}
			else
			{
				Application.Run(new FileBrowser());
			}
		}
	}
}