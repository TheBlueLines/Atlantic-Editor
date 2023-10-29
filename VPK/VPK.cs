using System.Text;

namespace TTMC.VPK
{
	public class VPK
	{
		public List<Entry> entries = new();
		internal byte[] data = new byte[0];
		internal byte[] tree = new byte[0];
		internal string origin = string.Empty;
		public VPK(string filePath, bool force = false)
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
				int headerLength = 0;
				switch (version)
				{
					case 1:
						headerLength = 12;
						break;
					case 2:
						headerLength = 28;
						break;
					default:
						return;
				}
				tree = data[headerLength..][..treeSize];
				int pointer = 0;
				while (true)
				{
					string extension = NullTerminated(tree, pointer);
					pointer += extension.Length + 1;
					if (string.IsNullOrEmpty(extension))
					{
						break;
					}
					while (true)
					{
						string path = NullTerminated(tree, pointer);
						pointer += path.Length + 1;
						if (string.IsNullOrEmpty(path))
						{
							break;
						}
						while (true)
						{
							string filename = NullTerminated(tree, pointer);
							pointer += filename.Length + 1;
							if (string.IsNullOrEmpty(filename))
							{
								break;
							}
							ushort preloadBytes = BitConverter.ToUInt16(tree, pointer + 4);
							Entry entry = new(this, pointer, extension, path, filename);
							response.Add(entry);
							pointer += preloadBytes + 18;
						}
					}
				}
			}
			entries = response;
		}
		internal string NullTerminated(byte[] data, int start = 0, int max = 0)
		{
			List<byte> bytes = (max != 0 ? data[start..][..max] : data[start..]).ToList();
			int count = bytes.Contains(0x00) ? bytes.IndexOf(0x00) : max;
			return Encoding.ASCII.GetString(data, start, count);
		}
		public string AlterFile(int index)
		{
			string temp = index.ToString();
			while (temp.Length < 3)
			{
				temp = "0" + temp;
			}
			return origin + "_" + temp + ".vpk";
		}
		public byte[] Combine(params byte[][] arrays)
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
		internal VPK vpk;
		internal int pointer;
		public string extension;
		public string path;
		public string filename;
		public Entry(VPK vpk, int pointer, string extension, string path, string filename)
		{
			this.vpk = vpk;
			this.pointer = pointer;
			this.extension = extension;
			this.path = path;
			this.filename = filename;
		}
		public int CRC
		{
			get
			{
				return BitConverter.ToInt32(vpk.tree, pointer);
			}
		}
		internal ushort preloadBytes
		{
			get
			{
				return BitConverter.ToUInt16(vpk.tree, pointer + 4);
			}
		}
		internal ushort archiveIndex
		{
			get
			{
				return BitConverter.ToUInt16(vpk.tree, pointer + 6);
			}
		}
		internal int entryOffset
		{
			get
			{
				return BitConverter.ToInt32(vpk.tree, pointer + 8);
			}
		}
		internal int entryLength
		{
			get
			{
				return BitConverter.ToInt32(vpk.tree, pointer + 12);
			}
		}
		public byte[] preloadData
		{
			get
			{
				return vpk.data[(pointer + 18)..][..preloadBytes];
			}
		}
		public string fullPath
		{
			get
			{
				return  path == " " ? filename + "." + extension : path + "/" + filename + "." + extension;
			}
		}
		public byte[] data
		{
			get
			{
				if (entryLength > 0)
				{
					return vpk.Combine(preloadData, File.ReadAllBytes(vpk.AlterFile(archiveIndex))[entryOffset..][..entryLength]);
				}
				return preloadData;
			}
		}
	}
}