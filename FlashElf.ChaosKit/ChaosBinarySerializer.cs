using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	public class ChaosBinarySerializer : IChaosSerializer
	{
		public byte[] Serialize<T>(T obj)
		{
			return BuiltBinarySerializer.Serialize(obj);
		}

		public object Deserialize(System.Type type, byte[] data)
		{
			return BuiltBinarySerializer.Deserialize(type, data);
		}
	}
}