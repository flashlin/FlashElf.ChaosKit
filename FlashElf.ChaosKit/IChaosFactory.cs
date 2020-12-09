using System;
using System.Reflection;
using FlashElf.ChaosKit.Protos;
using Google.Protobuf;

namespace FlashElf.ChaosKit
{
	public interface IChaosFactory
	{
		TRepoInterface CreateChaosService<TRepoInterface>()
			where TRepoInterface : class;

		ChaosInvocation CreateChaosInvocation(Type implementType, 
			MethodInfo invocationMethod,
			object[] invocationArguments);

		ChaosRequest CreateChaosRequest(ChaosInvocation invocation);
		ChaosInvocationResp GetInvocationResp(ChaosReply reply);
		ChaosInvocation GetChaosInvocationFrom(ChaosRequest request);
		ChaosReply CreateChaosReply(string returnTypeFullName, object returnValue);
	}
}