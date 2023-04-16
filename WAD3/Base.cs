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
		public Texture texture
		{
			get
			{
				return new() { entry = this };
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
		internal Entry entry = new();
		internal int? mipOffset0
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					return BitConverter.ToInt32(Core.bytes, entry.textureOffset + 24);
				}
				return null;
			}
		}
		internal int? mipOffset1
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					return BitConverter.ToInt32(Core.bytes, entry.textureOffset + 28);
				}
				return null;
			}
		}
		internal int? mipOffset2
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					return BitConverter.ToInt32(Core.bytes, entry.textureOffset + 32);
				}
				return null;
			}
		}
		internal int? mipOffset3
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					return BitConverter.ToInt32(Core.bytes, entry.textureOffset + 36);
				}
				return null;
			}
		}
		public string? name
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					return Core.NullTerminated(Core.bytes, entry.textureOffset);
				}
				return null;
			}
		}
		public List<Color>? palette
		{
			get
			{
				if (Core.bytes != null)
				{
					int paletteOffset = entry.textureOffset + entry.size - (256 * 3) - 2;
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
				return BitConverter.ToInt32(Core.bytes, entry.textureOffset + (entry.type == 0x40 || entry.type == 0x43 ? 20 : 4));
			}
		}
		public int width
		{
			get
			{
				return BitConverter.ToInt32(Core.bytes, entry.textureOffset + (entry.type == 0x40 || entry.type == 0x43 ? 16 : 0));
			}
		}
		public byte[]? texture0
		{
			get
			{
				if (entry.type == 0x40 || entry.type == 0x43)
				{
					return mipOffset0 != null ? Core.bytes[(entry.textureOffset + mipOffset0.Value)..][..(width * height)] : null;
				}
				return Core.bytes[(entry.textureOffset + 8)..][..(width * height)];
			}
		}
		public byte[]? texture1
		{
			get
			{
				return mipOffset1 != null ? Core.bytes[(entry.textureOffset + mipOffset1.Value)..][..(width / 2 * height / 2)] : null;
			}
		}
		public byte[]? texture2
		{
			get
			{
				return mipOffset2 != null ? Core.bytes[(entry.textureOffset + mipOffset2.Value)..][..(width / 4 * height / 4)] : null;
			}
		}
		public byte[]? texture3
		{
			get
			{
				return mipOffset3 != null ? Core.bytes[(entry.textureOffset + mipOffset3.Value)..][..(width / 8 * height / 8)] : null;
			}
		}
	}
}