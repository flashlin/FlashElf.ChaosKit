using System;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using T1.Standard.Common;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	using ChaosProtoClient = ChaosProto.ChaosProtoClient;
	public class ChaosClient : IDisposable
	{
		private readonly Channel _channel;
		private readonly ChaosProtoClient _client;
		private readonly IChaosSerializer _serializer;
		private readonly TypeFinder _typeFinder;
		private readonly ChaosBinarySerializer _binarySerializer;

		public ChaosClient(string chaosServer, IChaosSerializer serializer)
		{
			_serializer = serializer;
			_binarySerializer = new ChaosBinarySerializer();
			_channel = new Channel(chaosServer, ChannelCredentials.Insecure);
			_client = new ChaosProtoClient(_channel);
			_typeFinder = new TypeFinder();
		}

		public object Send(ChaosRepoInvocation invocation)
		{
			var req = new ChaosRequest()
			{
				InterfaceName = invocation.InterfaceName,
				MethodName = invocation.MethodName,
				Parameters = ByteString.CopyFrom(_serializer.Serialize(invocation.Parameters))
			};
			
			var reply = _client.Send(req);
			var data = reply.Data.ToByteArray();

			var invocationResp = (ChaosInvocationResp)_binarySerializer.Deserialize(typeof(ChaosInvocationResp), data);
			var dataType = _typeFinder.Find(invocationResp.DataTypeFullName);
			return _serializer.Deserialize(dataType, invocationResp.Data);
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