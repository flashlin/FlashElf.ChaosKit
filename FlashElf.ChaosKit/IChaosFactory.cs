using System;
using System.Reflection;
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
	}
}