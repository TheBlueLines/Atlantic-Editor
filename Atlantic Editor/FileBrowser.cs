﻿using TTMC.VPK;

namespace Atlantic_Editor
{
	public partial class FileBrowser : Form
	{
		VPK? vpk = null;
		Stream? stream = null;
		string? fileName = null;
		public FileBrowser(string? fileName = null)
		{
			this.fileName = fileName;
			InitializeComponent();
			if (!string.IsNullOrEmpty(fileName))
			{
				stream = File.OpenRead(fileName);
				vpk = new(stream);
				UpdateTree();
			}
		}
		private void UpdateTree()
		{
			tree.BeginUpdate();
			if (vpk != null)
			{
				tree.Nodes.AddRange(Eclipse(vpk.entries));
			}
			tree.EndUpdate();
		}
		private TreeNode[] Eclipse(IEnumerable<Entry> entries, int depth = 0)
		{
			List<TreeNode> nodes = new();
			List<string> keys = new();
			foreach (Entry entry in entries)
			{
				string[] tmp = entry.path.Split('/');
				if (tmp.Count() > depth)
				{
					keys.Add(tmp[depth]);
				}
				else
				{
					nodes.Add(new(entry.filename + "." + entry.extension));
				}
			}
			foreach (string value in keys.Distinct())
			{
				nodes.Add(new(value, Eclipse(entries.Where(x => x.path.Split('/').Count() > depth && x.path.Split('/')[depth] == value), depth + 1)));
			}
			return nodes.ToArray();
		}
		private string[] DoWork(TreeNodeCollection treeNodeCollection)
		{
			List<string> strings = new();
			foreach (TreeNode node in treeNodeCollection)
			{
				if (node.Nodes.Count > 0)
				{
					strings.AddRange(DoWork(node.Nodes));
				}
				else
				{
					strings.Add(node.FullPath);
				}
			}
			return strings.ToArray();
		}
		private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (vpk != null)
			{
				if (tree.SelectedNode.Nodes.Count > 0)
				{
					FolderBrowserDialog dialog = new()
					{
						ShowNewFolderButton = true
					};
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						string path = tree.SelectedNode.FullPath;
						foreach (Entry entry in vpk.entries.Where(x => x.fullPath.StartsWith(path.Replace('\\', '/'))))
						{
							Directory.CreateDirectory(dialog.SelectedPath + "\\" + (entry.path ?? string.Empty)[path.Length..]);
							File.WriteAllBytes(dialog.SelectedPath + "\\" + entry.fullPath.Replace('/', '\\')[path.Length..], GetData(entry));
						}
					}
				}
				else
				{
					SaveFileDialog dialog = new()
					{
						FileName = Path.GetFileName(tree.SelectedNode.FullPath)
					};
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						string temp = tree.SelectedNode.FullPath;
						Entry? entry = vpk.entries.Where(x => x.fullPath == temp.Replace('\\', '/')).FirstOrDefault();
						if (entry != null)
						{
							File.WriteAllBytes(dialog.FileName, GetData(entry));
						}
					}
				}
			}
		}
		private void allToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (vpk != null)
			{
				FolderBrowserDialog dialog = new()
				{
					ShowNewFolderButton = true
				};
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					foreach (Entry entry in vpk.entries)
					{
						Directory.CreateDirectory(dialog.SelectedPath + "\\" + entry.path);
						File.WriteAllBytes(dialog.SelectedPath + "\\" + entry.fullPath.Replace('/', '\\'), GetData(entry));
					}
				}
			}
		}
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new()
			{
				Multiselect = false
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(dialog.FileName))
				{
					if (Path.GetExtension(dialog.FileName).ToLower() == ".wad")
					{
						TextureBrowser textureBrowser = new(dialog.FileName);
						textureBrowser.Show();
					}
					else
					{
						vpk = new(File.OpenRead(dialog.FileName));
						UpdateTree();
					}
				}
			}
		}
		private byte[] GetData(Entry entry)
		{
			if (vpk != null && stream != null && !string.IsNullOrEmpty(fileName))
			{
				byte[] bytes = new byte[entry.entryLength];
				if (entry.archiveIndex == 0x7fff)
				{
					stream.Read(bytes, entry.entryOffset + vpk.treeSize + (vpk.version == 1 ? 12 : 28), entry.entryLength);
					return bytes;
				}
				else
				{
					FileStream fileStream = File.OpenRead(fileName[..^7] + Better(entry.archiveIndex) + ".vpk");
					fileStream.Position = entry.entryOffset;
					fileStream.Read(bytes, 0, entry.entryLength);
					fileStream.Close();
					fileStream.Dispose();
					return bytes;
				}
			}
			return [];
		}
		private string Better(int num)
		{
			string start = num.ToString();
			while (start.Length < 3)
			{
				start = "0" + start;
			}
			return start;
		}
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Atlantic Editor v0.2\nMade by TheBlueLiens", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}