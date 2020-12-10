using FlashElf.ChaosKit.Protos;

namespace FlashElf.ChaosKit
{
	public static class AnyProtoExtension
	{
		static readonly AnyProtoConvert AnyProtoConvert = new AnyProtoConvert();

		public static AnyProto ToAnyProto<T>(this T obj)
		{
			return AnyProtoConvert.ToAnyProto(obj);
		}

		public static T ConvertTo<T>(this AnyProto anyProto)
		{
			return AnyProtoConvert.From<T>(anyProto);
		}
	}
}