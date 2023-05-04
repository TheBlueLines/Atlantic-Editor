using System.Text;

namespace TTMC.VPK
{
	public class VPK
	{
		public static List<Entry> entries = new();
		internal static byte[] data = new byte[0];
		internal static string origin = string.Empty;
		public static void LoadVPK(string filePath, bool force = false)
		{
			int ant = filePath.LastIndexOf("_");
			origin = filePath[..ant];
			data = File.ReadAllBytes(filePath);
			List<Entry> response = new();
			int signature = BitConverter.ToInt32(data, 0);
			int version = BitConverter.ToInt32(data, 4);
			int treeSize = BitConverter.ToInt32(data, 8);
			if (signature == 1437209140 || force)
			{
				int pointer = 0;
				if (version == 1)
				{
					pointer += 12;
					while (true)
					{
						string extension = NullTerminated(data, pointer);
						pointer += extension.Length + 1;
						if (string.IsNullOrEmpty(extension))
						{
							break;
						}
						while (true)
						{
							string path = NullTerminated(data, pointer);
							pointer += path.Length + 1;
							if (string.IsNullOrEmpty(path))
							{
								break;
							}
							while (true)
							{
								string filename = NullTerminated(data, pointer);
								pointer += filename.Length + 1;
								if (string.IsNullOrEmpty(filename))
								{
									break;
								}
								ushort preloadBytes = BitConverter.ToUInt16(data, pointer + 4);
								Entry entry = new()
								{
									extension = extension,
									path = path,
									filename = filename,
									pointer = pointer
								};
								response.Add(entry);
								pointer += preloadBytes + 18;
							}
						}
					}
				}
			}
			entries = response;
		}
		public static string NullTerminated(byte[] data, int start = 0)
		{
			return Encoding.ASCII.GetString(data, start, data[start..].ToList().IndexOf(0x00));
		}
		public static string AlterFile(int index)
		{
			string temp = index.ToString();
			while (temp.Length < 3)
			{
				temp = "0" + temp;
			}
			return origin + "_" + temp + ".vpk";
		}
		public static byte[] Combine(params byte[][] arrays)
		{
			byte[] rv = new byte[arrays.Sum(a => a.Length)];
			int offset = 0;
			foreach (byte[] array in arrays)
			{
				Buffer.BlockCopy(array, 0, rv, offset, array.Length);
				offset += array.Length;
			}
			return rv;
		}
	}
	public class Entry
	{
		internal int pointer { get; set; }
		public string? extension { get; set; }
		public string? path { get; set; }
		public string? filename { get; set; }
		public int CRC
		{
			get
			{
				return BitConverter.ToInt32(VPK.data, pointer);
			}
		}
		internal ushort preloadBytes
		{
			get
			{
				return BitConverter.ToUInt16(VPK.data, pointer + 4);
			}
		}
		internal ushort archiveIndex
		{
			get
			{
				return BitConverter.ToUInt16(VPK.data, pointer + 6);
			}
		}
		internal int entryOffset
		{
			get
			{
				return BitConverter.ToInt32(VPK.data, pointer + 8);
			}
		}
		internal int entryLength
		{
			get
			{
				return BitConverter.ToInt32(VPK.data, pointer + 12);
			}
		}
		public byte[] preloadData
		{
			get
			{
				return VPK.data[(pointer + 18)..][..preloadBytes];
			}
		}
		public string fullPath
		{
			get
			{
				return  path + "/" + filename + "." + extension;
			}
		}
		public byte[] data
		{
			get
			{
				if (entryLength > 0)
				{
					return VPK.Combine(preloadData, File.ReadAllBytes(VPK.AlterFile(archiveIndex))[entryOffset..][..entryLength]);
				}
				return preloadData;
			}
		}
	}
}