using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using T1.Standard.Common;

namespace FlashElf.ChaosKit
{
	public static class ChaosParameterListExtension
	{
		public static List<ChaosParameter> GetChaosParameters(this MethodInfo invocationMethod, 
			IChaosSerializer serializer,
			object[] invocationArguments)
		{
			var args = new List<ChaosParameter>();
			var parameters = invocationMethod.GetParameters();
			for (var i = 0; i < invocationArguments.Length; i++)
			{
				var parameter = new ChaosParameter()
				{
					ParameterType = parameters[i].ParameterType.FullName,
					Name = parameters[i].Name,
					Value = serializer.Serialize(invocationArguments[i])
				};
				args.Add(parameter);
			}
			return args;
		}

		public static IEnumerable<object> DeserializeToArguments(this List<ChaosParameter> requestParameters,
			IChaosSerializer serializer,
			TypeFinder typeFinder)
		{
			foreach (var requestParameter in requestParameters)
			{
				var parameterType = typeFinder.Find(requestParameter.ParameterType);
				var value = serializer.Deserialize(parameterType, requestParameter.Value);
				yield return value;
			}
		}
	}
}