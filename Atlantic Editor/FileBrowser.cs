using TTMC.VPK;

namespace Atlantic_Editor
{
    public partial class FileBrowser : Form
    {
        public FileBrowser(string? fileName = null)
        {
            InitializeComponent();
            tree.BeginUpdate();
            tree.EndUpdate();
            if (!string.IsNullOrEmpty(fileName))
            {
                VPK.LoadVPK(fileName);
                UpdateTree();
            }
        }
        private void UpdateTree()
        {
            foreach (Entry entry in VPK.entries)
            {
                TreeNodeCollection selected = tree.Nodes;
                string[] basic = entry.fullPath.Split('/');
                foreach (string value in basic)
                {
                    if (!selected.ContainsKey(value))
                    {
                        TreeNode treeNode = selected.Add(value, value);
                        selected = treeNode.Nodes;
                    }
                    else
                    {
                        selected = selected[selected.IndexOfKey(value)].Nodes;
                    }
                }
            }
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
            if (tree.SelectedNode.Nodes.Count > 0)
            {
                FolderBrowserDialog dialog = new()
                {
                    ShowNewFolderButton = true
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = tree.SelectedNode.FullPath;
                    foreach (Entry entry in VPK.entries.Where(x => x.fullPath.StartsWith(path.Replace('\\', '/'))))
                    {
                        Directory.CreateDirectory(dialog.SelectedPath + "\\" + (entry.path ?? string.Empty)[path.Length..]);
                        File.WriteAllBytes(dialog.SelectedPath + "\\" + entry.fullPath.Replace('/', '\\')[path.Length..], entry.data);
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
                    Entry? entry = VPK.entries.Where(x => x.fullPath == temp.Replace('\\', '/')).FirstOrDefault();
                    if (entry != null)
                    {
                        File.WriteAllBytes(dialog.FileName, entry.data);
                    }
                }
            }
        }
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new()
            {
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Entry entry in VPK.entries)
                {
                    Directory.CreateDirectory(dialog.SelectedPath + "\\" + entry.path);
                    File.WriteAllBytes(dialog.SelectedPath + "\\" + entry.fullPath.Replace('/', '\\'), entry.data);
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
                    VPK.LoadVPK(dialog.FileName);
                    UpdateTree();
                }
            }
        }
    }
}