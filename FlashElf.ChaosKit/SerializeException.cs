using System;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class SerializeException
	{
		public static SerializeException CreateFromException(Exception ex)
		{
			return new SerializeException()
			{
				Message = ex.Message,
				StackTrace = ex.StackTrace,
			};
		}
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public override string ToString()
		{
			return $"{Message}";
		}
	}
}