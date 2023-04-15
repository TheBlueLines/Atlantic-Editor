using System.Drawing;
using System.Globalization;
using System.Text;

namespace TTMC.WAD3
{
    public class Handle
    {
        public virtual void SetTextureAll(int number) { }
        public virtual void SetTextureLoad(int number) { }
        public virtual void NewTexture(Texture texture) { }
        public virtual void Textures(List<Texture> textures) { }
    }
    public class Core
    {
        public static Handle handle = new();
        public static void LoadWAD(byte[] data, bool force = false)
        {
            List<Texture> list = new();
            string szMagic = Encoding.UTF8.GetString(data, 0, 4);
            int nDir = BitConverter.ToInt32(data, 4);
            int nDirOffset = BitConverter.ToInt32(data, 8);
            if (szMagic == "WAD3" || force)
            {
                handle.SetTextureAll(nDir);
                for (int i = 0; i < nDir; i++)
                {
                    // Texture Details
                    int temp = (int)nDirOffset + i * 32;
                    int textureFileOffset = BitConverter.ToInt32(data, temp);
                    int textureFileDiskSize = BitConverter.ToInt32(data, temp + 4);
                    int textureFileSize = BitConverter.ToInt32(data, temp + 8);
                    byte nType = data[temp + 12];
                    bool bCompression = BitConverter.ToBoolean(data, temp + 13);
                    short padding = BitConverter.ToInt16(data, temp + 14);
                    string name = NullTerminated(data, temp + 16);
                    // Texure File
                    string szName = NullTerminated(data, textureFileOffset);
                    int nWidth = BitConverter.ToInt32(data, textureFileOffset + 16);
                    int nHeight = BitConverter.ToInt32(data, textureFileOffset + 20);
                    int mipOffset0 = BitConverter.ToInt32(data, textureFileOffset + 24);
                    int mipOffset1 = BitConverter.ToInt32(data, textureFileOffset + 28);
                    int mipOffset2 = BitConverter.ToInt32(data, textureFileOffset + 32);
                    int mipOffset3 = BitConverter.ToInt32(data, textureFileOffset + 36);
                    byte[] texture0 = data[(textureFileOffset + mipOffset0)..][..(nWidth * nHeight)];
                    byte[] texture1 = data[(textureFileOffset + mipOffset1)..][..(nWidth / 2 * nHeight / 2)];
                    byte[] texture2 = data[(textureFileOffset + mipOffset2)..][..(nWidth / 4 * nHeight / 4)];
                    byte[] texture3 = data[(textureFileOffset + mipOffset3)..][..(nWidth / 8 * nHeight / 8)];
                    int paletteOffset = textureFileOffset + textureFileSize - (256 * 3) - 2;
                    byte[] palette = data[paletteOffset..][..(256 * 3)];
                    // Color List
                    List <Color> colors = new();
                    for (int j = 0; j < palette.Length; j += 3)
                    {
                        colors.Add(Color.FromArgb(palette[j], palette[j + 1], palette[j + 2]));
                    }
                    // Making an element
                    Texture element = new()
                    {
                        name = name,
                        title = szName,
                        palette = colors,
                        texture0 = texture0,
                        texture1 = texture1,
                        texture2 = texture2,
                        texture3 = texture3,
                        height = nHeight,
                        width = nWidth,
                        diskSize = textureFileDiskSize,
                        compressed = bCompression,
                        type = nType
                    };
                    list.Add(element);
                    handle.NewTexture(element);
                    handle.SetTextureLoad(i);
                }
            }
            handle.Textures(list);
        }
        private static string NullTerminated(byte[] data, int start = 0)
        {
            return Encoding.ASCII.GetString(data, start, data[start..].ToList().IndexOf(0x00));
        }
    }
    public class Texture
    {
        public string? name { get; set; }
        public string? title { get; set; }
        public List<Color>? palette { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public bool compressed { get; set; }
        public int diskSize { get; set; }
        public byte[]? texture0 { get; set; }
        public byte[]? texture1 { get; set; }
        public byte[]? texture2 { get; set; }
        public byte[]? texture3 { get; set; }
        public byte? type { get; set; }
    }
}