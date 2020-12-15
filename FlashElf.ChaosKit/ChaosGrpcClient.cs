using System;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;
using T1.Standard.Common;
using T1.Standard.Serialization;

namespace FlashElf.ChaosKit
{
	using ChaosProtoClient = ChaosProto.ChaosProtoClient;
	public class ChaosGrpcClient : IDisposable, IChaosClient
	{
		private readonly Channel _channel;
		private readonly ChaosProtoClient _client;
		private readonly IChaosConverter _chaosConverter;

		public ChaosGrpcClient(IOptions<ChaosClientConfig> config,
			IChaosConverter chaosConverter)
		{
			_chaosConverter = chaosConverter;
			_channel = new Channel(config.Value.ChaosServerIp, ChannelCredentials.Insecure);
			_client = new ChaosProtoClient(_channel);
		}

		public object Send(ChaosInvocation invocation)
		{
			var req = invocation.ToAnyProto();

			var reply = _client.SendInvocation(req);

			var invocationResp = reply.ConvertTo<ChaosInvocationResp>();

			if (invocationResp.DataTypeFullName == null)
			{
				return null;
			}

			return _chaosConverter.ToData(invocationResp);
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

		~ChaosGrpcClient()
		{
			ReleaseUnmanagedResources();
		}
	}
}