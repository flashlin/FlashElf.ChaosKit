using FlashElf.ChaosKit.Protos;
using Google.Protobuf;

namespace FlashElf.ChaosKit
{
	public class AnyProtoConvert
	{
		private readonly ChaosBinarySerializer _binarySerializer;

		public AnyProtoConvert()
		{
			_binarySerializer = new ChaosBinarySerializer();
		}

		public AnyProto ToAnyProto<T>(T obj)
		{
			return new AnyProto()
			{
				Data = ByteString.CopyFrom(_binarySerializer.Serialize(obj))
			};
		}

		public T From<T>(AnyProto anyProto)
		{
			var byteArray = anyProto.Data.ToByteArray();
			return (T)_binarySerializer.Deserialize(typeof(T), byteArray);
		}
	}
}