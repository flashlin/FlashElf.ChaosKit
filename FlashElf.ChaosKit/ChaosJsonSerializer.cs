using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using T1.Standard.Common;
using T1.Standard.DynamicCode;
using T1.Standard.Extensions;

namespace FlashElf.ChaosKit
{
	public class ChaosJsonSerializer : IChaosSerializer
	{
		private readonly ChaosBinarySerializer _binarySerializer;

		public ChaosJsonSerializer()
		{
			_binarySerializer = new ChaosBinarySerializer();
		}

		public byte[] Serialize<T>(T obj)
		{
			var json = typeof(string) == typeof(T) ? $"{obj}" : JsonSerializer.Serialize(obj);
			return Encoding.UTF8.GetBytes(json);
		}

		public object Deserialize(Type type, byte[] data)
		{
			var json = Encoding.UTF8.GetString(data);
			if (typeof(string) == type)
			{
				return json;
			}
			return JsonSerializer.Deserialize(json, type);
		}
	}
}