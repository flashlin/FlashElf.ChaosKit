using System;
using System.Linq;
using FlashElf.ChaosKit;
using T1.Standard.Common;
using Xunit;

namespace ChaosKitTest
{
	public class SerializeTest
	{
		[Fact]
		public void Serialize_String()
		{
			var text = "abc";
			var jsonSerializer = new ChaosJsonSerializer();
			var json = jsonSerializer.Serialize(text);
			var actual = jsonSerializer.Deserialize(typeof(string), json);
			Assert.Equal(text, actual);
		}

		[Fact]
		public void Serialize_and_deserialize_ChaosInvocation()
		{
			var typeFinder = new TypeFinder();
			var serializer = new ChaosJsonSerializer();
			var method = typeof(MyService).GetMethod(nameof(MyService.SayHello));

			var invocation = new ChaosInvocation();
			object[] arguments = { "Flash" };
			invocation.Parameters = method.GetChaosParameters(serializer, arguments);


			var binarySerializer = new ChaosBinarySerializer();
			var invocationData = binarySerializer.Serialize(invocation);

			var obj = (ChaosInvocation)binarySerializer.Deserialize(typeof(ChaosInvocation), invocationData);
			var paramters = obj.Parameters.DeserializeToArguments(serializer, typeFinder)
				.ToArray();
			Assert.Equal("Flash", paramters[0]);
		}

		[Fact]
		public void Serialize_and_deserialize_ChaosInvocation_null()
		{
			var typeFinder = new TypeFinder();
			var serializer = new ChaosJsonSerializer();
			var method = typeof(MyService).GetMethod(nameof(MyService.SayHello));

			var invocation = new ChaosInvocation();
			object[] arguments = { null };
			invocation.Parameters = method.GetChaosParameters(serializer, arguments);


			var binarySerializer = new ChaosBinarySerializer();
			var invocationData = binarySerializer.Serialize(invocation);

			var obj = (ChaosInvocation)binarySerializer.Deserialize(typeof(ChaosInvocation), invocationData);
			var paramters = obj.Parameters.DeserializeToArguments(serializer, typeFinder)
				.ToArray();
			Assert.Equal(String.Empty, paramters[0]);
		}
	}
}
