using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using T1.Standard.DynamicCode;
using T1.Standard.Extensions;

namespace FlashElf.ChaosKit
{
	public class ChaosJsonSerializer : IChaosSerializer
	{
		public byte[] Serialize<T>(T obj)
		{
			return JsonSerializer.Serialize(obj).ToByteArray();
		}

		public object Deserialize(Type type, byte[] data)
		{
			var deserialize = DynamicMethod.GetGenericMethod(
				typeof(JsonSerializer),
				new[] { type },
				nameof(JsonSerializer.Deserialize),
				new Type[] { typeof(string), typeof(JsonSerializerOptions) });
			var json = Encoding.ASCII.GetString(data);
			return deserialize(null, new object[] { json });
		}
	}
}