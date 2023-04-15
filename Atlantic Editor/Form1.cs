using System.Drawing.Drawing2D;
using TTMC.WAD3;

namespace Atlantic_Editor
{
    public partial class Main : Form
    {
        public Main(string? firstLoad = null)
        {
            InitializeComponent();
            if (firstLoad != null)
            {
                LoadFile(firstLoad);
            }
        }
        private void LoadFile(string path)
        {
            list.Items.Clear();
            byte[] file = File.ReadAllBytes(path);
            Core.LoadWAD(file);
            ReloadList();
        }
        public void ReloadList(string? search = null)
        {
            try
            {
                list.Items.Clear();
                foreach (Entry entry in Core.entries)
                {
                    if (!string.IsNullOrEmpty(entry.name) && (string.IsNullOrEmpty(search) || entry.name.Contains(search)))
                    {
                        list.Items.Add(entry.name);
                    }
                }
            }
            catch { }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list.SelectedIndex >= 0)
            {
                Texture? texture = Core.entries[list.SelectedIndex].texture;
                if (texture != null)
                {
                    Bitmap image = new Bitmap(texture.width, texture.height);
                    List<Color>? palette = texture.palette;
                    byte[]? nzx = texture.texture0;
                    if (nzx != null && palette != null)
                    {
                        for (int i = 0; i < texture.height * texture.width; i++)
                        {
                            image.SetPixel(i % texture.width, i / texture.width, palette[nzx[i]]);
                        }
                    }
                    picture.Image = image;
                }
            }
        }
        private void picture_Paint(object sender, PaintEventArgs e)
        {
            if (picture.Image != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                e.Graphics.DrawImage(picture.Image, new Rectangle(0, 0, picture.Width, picture.Height), 0, 0, picture.Image.Width, picture.Image.Height, GraphicsUnit.Pixel);
            }
        }
        private void search_TextChanged(object sender, EventArgs e)
        {
            ReloadList(search.Text);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog1.FileName);
            }
        }
    }
}