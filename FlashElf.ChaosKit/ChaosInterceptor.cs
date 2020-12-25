using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace FlashElf.ChaosKit
{
	public class ChaosInterceptor : IInterceptor
	{
		private readonly Type _implementType;
		private readonly IChaosFactory _chaosFactory;
		private readonly IChaosPromiseInvocationSubjects _subjects;

		public ChaosInterceptor(Type implementType, IChaosFactory chaosFactory,
			IChaosPromiseInvocationSubjects subjects)
		{
			_subjects = subjects;
			_chaosFactory = chaosFactory;
			_implementType = implementType;
		}

		public void Intercept(IInvocation invocation)
		{
			var chaosInvocation = _chaosFactory.CreateChaosInvocation(_implementType, invocation.Method, invocation.Arguments);
			invocation.ReturnValue = _subjects.Call(chaosInvocation);
		}
	}
}