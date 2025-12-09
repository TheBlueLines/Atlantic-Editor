namespace TTMC.MPQ
{
	public class MPQ
	{
		public readonly uint magic;
		public readonly uint headerSize;
		public readonly uint archiveSize;
		public readonly ushort formatVersion;
		public readonly ushort blockSize;
		public readonly uint hashTablePosition;
		public readonly uint blockTablePosition;
		public readonly uint hashTableSize;
		public readonly uint blockTableSize;
		public MPQ(Stream stream)
		{
			byte[] rawData = new byte[4];
			stream.Read(rawData, 0, 4);
			magic = BitConverter.ToUInt32(rawData);
			if (magic == 0x1A51504D)
			{
				stream.Read(rawData, 0, 4);
				headerSize = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 4);
				archiveSize = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 2);
				formatVersion = BitConverter.ToUInt16(rawData);
				stream.Read(rawData, 0, 2);
				blockSize = BitConverter.ToUInt16(rawData);
				stream.Read(rawData, 0, 4);
				hashTablePosition = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 4);
				blockTablePosition = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 4);
				hashTableSize = BitConverter.ToUInt32(rawData);
				stream.Read(rawData, 0, 4);
				blockTableSize = BitConverter.ToUInt32(rawData);
				return;
			}
			throw new("Unsupported file");
		}
	}
}