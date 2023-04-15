using System.Drawing;
using System.Text;

namespace TTMC.WAD3
{
	public class Core
	{
		public static List<Entry> entries = new();
		internal static byte[] bytes = new byte[0];
		public static void LoadWAD(byte[] data, bool force = false)
		{
			bytes = data;
			List<Entry> list = new();
			string szMagic = Encoding.UTF8.GetString(data, 0, 4);
			int nDir = BitConverter.ToInt32(data, 4);
			int nDirOffset = BitConverter.ToInt32(data, 8);
			if (szMagic == "WAD3" || force)
			{
				for (int i = 0; i < nDir; i++)
				{
					int temp = (int)nDirOffset + i * 32;
					list.Add(new() { offset = temp });
				}
			}
			entries = list;
		}
		public static string NullTerminated(byte[] data, int start = 0)
		{
			return Encoding.ASCII.GetString(data, start, data[start..].ToList().IndexOf(0x00));
		}
	}
	public class Entry
	{
		internal int offset = 0;
		internal int textureOffset
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset);
			}
		}
		public Texture? texture
		{
			get
			{
				return new() { offset = textureOffset, size = size };
			}
		}
		public int diskSize
		{
			get
			{
				return Core.bytes != null ? BitConverter.ToInt32(Core.bytes, offset + 4) : 0;
			}
		}
		public int size
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 8);
			}
		}
		public byte? type
		{
			get
			{
				return Core.bytes[offset + 12];
			}
		}
		public bool compression
		{
			get
			{
				return BitConverter.ToBoolean(Core.bytes, offset + 13);

			}
		}
		public string? name
		{
			get
			{
				return Core.NullTerminated(Core.bytes, offset + 16);
			}
		}
	}
	public class Texture
	{
		public int offset = 0;
		public int size = 0;
		internal int? mipOffset0
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 24);
			}
		}
		internal int? mipOffset1
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 28);
			}
		}
		internal int? mipOffset2
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 32);
			}
		}
		internal int? mipOffset3
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 36);
			}
		}
		public string? name
		{
			get
			{
				return Core.NullTerminated(Core.bytes, offset);
			}
		}
		public List<Color>? palette
		{
			get
			{
				if (Core.bytes != null)
				{
					int paletteOffset = offset + size - (256 * 3) - 2;
					byte[] palette = Core.bytes[paletteOffset..][..(256 * 3)];
					List<Color> colors = new();
					for (int j = 0; j < palette.Length; j += 3)
					{
						colors.Add(Color.FromArgb(palette[j], palette[j + 1], palette[j + 2]));
					}
					return colors;
				}
				return null;
			}
		}
		public int height
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 20);
			}
		}
		public int width
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, offset + 16);
			}
		}
		public byte[]? texture0
		{
			get
			{
				return mipOffset0 != null ? Core.bytes[(offset + mipOffset0.Value)..][..(width * height)] : null;
			}
		}
		public byte[]? texture1
		{
			get
			{
				return mipOffset1 != null ? Core.bytes[(offset + mipOffset1.Value)..][..(width / 2 * height / 2)] : null;
			}
		}
		public byte[]? texture2
		{
			get
			{
				return mipOffset2 != null ? Core.bytes[(offset + mipOffset2.Value)..][..(width / 4 * height / 4)] : null;
			}
		}
		public byte[]? texture3
		{
			get
			{
				return mipOffset3 != null ? Core.bytes[(offset + mipOffset3.Value)..][..(width / 8 * height / 8)] : null;
			}
		}
	}
}