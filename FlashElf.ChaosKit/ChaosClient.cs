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

		public object Send(ChaosInvocation invocation)
		{
			var req = new ChaosRequest()
			{
				Invocation = ByteString.CopyFrom(_binarySerializer.Serialize(invocation))
			};

			var reply = _client.Send(req);
			var data = reply.Data.ToByteArray();

			var invocationResp = (ChaosInvocationResp)_binarySerializer.Deserialize(typeof(ChaosInvocationResp), data);

			if (invocationResp.DataTypeFullName == null)
			{
				return null;
			}

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