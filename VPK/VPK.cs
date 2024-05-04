using System.Text;

namespace TTMC.VPK
{
	public class VPK
	{
		public readonly int signature;
		public readonly int version;
		public readonly int treeSize;
		public readonly int FileDataSectionSize;
		public readonly int ArchiveMD5SectionSize;
		public readonly int OtherMD5SectionSize;
		public readonly int SignatureSectionSize;
		public List<Entry> entries = new();
		private Stream stream;
		internal string origin = string.Empty;
		public VPK(Stream stream)
		{
			this.stream = stream;
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			signature = BitConverter.ToInt32(data);
			stream.Read(data, 0, 4);
			version = BitConverter.ToInt32(data);
			stream.Read(data, 0, 4);
			treeSize = BitConverter.ToInt32(data);
			if (version == 2)
			{
				stream.Read(data, 0, 4);
				FileDataSectionSize = BitConverter.ToInt32(data);
				stream.Read(data, 0, 4);
				ArchiveMD5SectionSize = BitConverter.ToInt32(data);
				stream.Read(data, 0, 4);
				OtherMD5SectionSize = BitConverter.ToInt32(data);
				stream.Read(data, 0, 4);
				SignatureSectionSize = BitConverter.ToInt32(data);
			}
			if (signature == 0x55AA1234)
			{
				while (stream.Position < (treeSize + 12))
				{
					string extension = NullTerminated();
					if (string.IsNullOrEmpty(extension))
					{
						break;
					}
					while (true)
					{
						string path = NullTerminated();
						if (string.IsNullOrEmpty(path))
						{
							break;
						}
						while (true)
						{
							string filename = NullTerminated();
							if (string.IsNullOrEmpty(filename))
							{
								break;
							}
							stream.Read(data, 0, 4);
							int crc = BitConverter.ToInt32(data);
							stream.Read(data, 0, 2);
							ushort preloadBytes = BitConverter.ToUInt16(data);
							stream.Read(data, 0, 2);
							ushort archiveIndex = BitConverter.ToUInt16(data);
							stream.Read(data, 0, 4);
							int entryOffset = BitConverter.ToInt32(data);
							stream.Read(data, 0, 4);
							int entryLength = BitConverter.ToInt32(data);
							byte[] preloadData = new byte[preloadBytes];
							stream.Read(preloadData, 0, preloadBytes);
							stream.Position += 2;
							Entry entry = new(extension, path, filename, crc, preloadBytes, archiveIndex, entryOffset, entryLength, preloadData);
							entries.Add(entry);
						}
					}
				}
			}
		}
		private string NullTerminated()
		{
			List<byte> buffer = new();
			while (true)
			{
				int b = stream.ReadByte();
				if (b == 0x00)
				{
					return Encoding.UTF8.GetString(buffer.ToArray());
				}
				buffer.Add((byte)b);
			}
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
		public readonly string extension;
		public readonly string path;
		public readonly string filename;
		public readonly int crc;
		public readonly ushort preloadBytes;
		public readonly ushort archiveIndex;
		public readonly int entryOffset;
		public readonly int entryLength;
		public readonly byte[] preloadData;
		public Entry(string extension, string path, string filename, int crc, ushort preloadBytes, ushort archiveIndex, int entryOffset, int entryLength, byte[] preloadData)
		{
			this.extension = extension;
			this.path = path;
			this.filename = filename;
			this.crc = crc;
			this.preloadBytes = preloadBytes;
			this.archiveIndex = archiveIndex;
			this.entryOffset = entryOffset;
			this.entryLength = entryLength;
			this.preloadData = preloadData;
		}
		public string fullPath
		{
			get
			{
				return  path == " " ? filename + "." + extension : path + "/" + filename + "." + extension;
			}
		}
	}
}