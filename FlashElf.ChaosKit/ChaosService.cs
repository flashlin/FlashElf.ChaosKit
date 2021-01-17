using System;
using System.Linq;
using System.Reflection;
using T1.Standard.Common;
using T1.Standard.DynamicCode;
using T1.Standard.Extensions;

namespace FlashElf.ChaosKit
{
	public class ChaosService : IChaosService
	{
		private readonly IChaosServiceResolver _serviceResolver;
		private readonly IChaosSerializer _serializer;
		private readonly TypeFinder _typeFinder;

		public ChaosService(IChaosSerializer serializer, IChaosServiceResolver serviceResolver)
		{
			_serializer = serializer;
			_serviceResolver = serviceResolver;
			_typeFinder = new TypeFinder();
		}

		public ChaosInvocationResp ProcessInvocation(ChaosInvocation chaosInvocation)
		{
			try
			{
				var realImplementObject = _serviceResolver.GetService(chaosInvocation.InterfaceTypeFullName);
				var realImplementType = realImplementObject.GetType();
				var realImplementInfo = ReflectionClass.Reflection(realImplementType);

				var mi = FindMethod(realImplementInfo, chaosInvocation);

				var requestParameters = chaosInvocation.Parameters;
				var args = requestParameters.DeserializeToArguments(_serializer, _typeFinder)
					.ToArray();

				var returnValue = mi.Func(realImplementObject, args);

				var invocationReply = ToChaosInvocationResp(chaosInvocation, returnValue);
				return invocationReply;
			}
			catch (Exception ex)
			{
				var invocationReply = ToChaosInvocationResp(chaosInvocation, (object)null);
				invocationReply.Exception = SerializeException.CreateFromException(ex);
				return invocationReply;
			}
		}

		private ChaosInvocationResp ToChaosInvocationResp(ChaosInvocation invocation,
			object returnValue)
		{
			var invocationReply = new ChaosInvocationResp()
			{
				DataTypeFullName = invocation.ReturnTypeFullName
			};

			if (invocation.IsReturnTask)
			{
				var taskResult = returnValue.GetTaskResult();
				invocationReply.Data = _serializer.Serialize(taskResult.value);
			}
			else
			{
				invocationReply.Data = _serializer.Serialize(returnValue);
			}

			return invocationReply;
		}

		private (MethodInfo MethodInfo, Func<object, object[], object> Func) FindMethod(ReflectionClass clazz,
			ChaosInvocation request)
		{
			foreach (var method in clazz.AllMethods)
			{
				if (!IsMatchMethod(method.MethodInfo, request)) continue;

				return (method.MethodInfo, method.Func);
			}

			foreach (var method in clazz.AllGenericMethods)
			{
				if (!IsMatchMethod(method.MethodInfo, request)) continue;

				var genericTypes = request.GenericTypes
					.Select(x => _typeFinder.Find(x.ParameterType))
					.ToArray();

				return (method.MethodInfo, method.GetFunc(genericTypes));
			}

			throw new EntryPointNotFoundException(request.MethodName);
		}

		private bool IsMatchMethod(MethodInfo methodInfo, ChaosInvocation request)
		{
			if (methodInfo.Name != request.MethodName)
			{
				return false;
			}

			var parameterInfos = methodInfo.GetParameters();
			if (parameterInfos.Length != request.Parameters.Count)
			{
				return false;
			}

			var isAllSame = parameterInfos.Zip(request.Parameters.Select(x => _typeFinder.Find(x.ParameterType)),
					(x, y) => x.ParameterType == y)
				.All(x => x == true);

			return isAllSame;
		}
	}
}