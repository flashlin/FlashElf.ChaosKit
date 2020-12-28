using T1.Standard.Common;

namespace FlashElf.ChaosKit
{
	public class ChaosConverter : IChaosConverter
	{
		private readonly IChaosSerializer _serializer;
		private readonly TypeFinder _typeFinder;

		public ChaosConverter(IChaosSerializer serializer)
		{
			_serializer = serializer;
			_typeFinder = new TypeFinder();
		}

		public object ToData(ChaosInvocationResp resp)
		{
			if (resp.Exception != null)
			{
				throw new RemotingException(resp.Exception);
			}

			if (string.IsNullOrEmpty(resp.DataTypeFullName))
			{
				return null;
			}
			var dataType = _typeFinder.Find(resp.DataTypeFullName);
			return _serializer.Deserialize(dataType, resp.Data);
		}
	}
}