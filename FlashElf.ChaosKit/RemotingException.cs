using System;

namespace FlashElf.ChaosKit
{
	public class RemotingException : Exception
	{
		public RemotingException(SerializeException ex)
			: base(ex.Message + Environment.NewLine + ex.StackTrace)
		{
		}
	}
}