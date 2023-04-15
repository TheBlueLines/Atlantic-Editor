using System.Drawing.Drawing2D;
using TTMC.WAD3;

namespace Atlantic_Editor
{
    public partial class Main : Form
    {
        public List<Texture> textures = new();
        private Task? task = null;
        public bool inSearch = false;
        public Main()
        {
            InitializeComponent();
        }
        private void LoadFile(string path)
        {
            textures.Clear();
            list.Items.Clear();
            Core.handle = new Handler(progressBar1, list, this);
            byte[] file = File.ReadAllBytes(path);
            task = new(() => Core.LoadWAD(file));
            task.Start();
        }
        public void ReloadList(string? search = null)
        {
            try
            {
                list.Items.Clear();
                foreach (Texture value in textures)
                {
                    if (!string.IsNullOrEmpty(value.title) && (string.IsNullOrEmpty(search) || value.title.Contains(search)))
                    {
                        list.Items.Add(value.title);
                    }
                }
            }
            catch { }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list.SelectedIndex >= 0)
            {
                Texture texture = textures[list.SelectedIndex];
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
            inSearch = !string.IsNullOrEmpty(search.Text);
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
    public class Handler : Handle
    {
        Main? main = null;
        ProgressBar nzx = new();
        ListBox listBox = new();
        int max = 0;
        public Handler(ProgressBar progressBar, ListBox list, Main form)
        {
            nzx = progressBar;
            main = form;
            listBox = list;
        }
        public override void SetTextureAll(int number)
        {
            nzx.Enabled = true;
            max = number;
        }
        public override void SetTextureLoad(int number)
        {
            nzx.Value = (int)((float)number / (float)max * 100);
        }
        public override void Textures(List<Texture> textures)
        {
            if (main != null)
            {
                main.textures = textures;
                main.ReloadList();
            }
            nzx.Value = 0;
            nzx.Enabled = false;
        }
        public override void NewTexture(Texture texture)
        {
            if (main != null)
            {
                if (!string.IsNullOrEmpty(texture.title) && !main.inSearch)
                {
                    listBox.Items.Add(texture.title);
                }
                main.textures.Add(texture);
            }
        }
    }
}