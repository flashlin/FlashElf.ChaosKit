using System;
using System.Reflection;
using Google.Protobuf;

namespace FlashElf.ChaosKit
{
	public interface IChaosFactory
	{
		TService CreateChaosService<TService>()
			where TService : class;

		ChaosInvocation CreateChaosInvocation(Type implementType, 
			MethodInfo invocationMethod,
			object[] invocationArguments);
	}
}