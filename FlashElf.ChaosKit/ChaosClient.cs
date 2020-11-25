using System;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	using ChaosProtoClient = ChaosProto.ChaosProtoClient;
	public class ChaosClient : IDisposable
	{
		private readonly Channel _channel;
		private readonly ChaosProtoClient _client;

		public ChaosClient(string chaosServer)
		{
			_channel = new Channel(chaosServer, ChannelCredentials.Insecure);
			_client = new ChaosProtoClient(_channel);
		}

		public object Send(ChaosRepoInvocation invocation)
		{
			var req = new ChaosRequest()
			{
				InterfaceName = invocation.InterfaceName,
				MethodName = invocation.MethodName,
				Parameters = ByteString.CopyFrom(BuiltBinarySerializer.Serialize(invocation.Parameters))
			};
			
			var reply = _client.Send(req);
			var data = reply.Data.ToByteArray();

			var invocationResp = (ChaosRepoInvocationResp)BuiltBinarySerializer.Deserialize(typeof(ChaosRepoInvocationResp), data);
			return invocationResp.Data;
		}

		public void Close()
		{
			_channel.ShutdownAsync().Wait();
		}

		private void ReleaseUnmanagedResources()
		{
			Close();
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~ChaosClient()
		{
			ReleaseUnmanagedResources();
		}
	}
}