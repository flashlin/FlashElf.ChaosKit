using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using T1.Standard.Extensions;

namespace FlashElf.ChaosKit
{
	[Serializable]
	public class SerializableDictionary
	{
		public string KeyTypeFullName { get; set; }
		public string ValueTypeFullName { get; set; }
		public List<SerializableKeyValuePair> Items { get; set; } = new List<SerializableKeyValuePair>();
	}

	public class PromiseInvocation
	{
		public AutoResetEvent WaitEvent { get; set; } = new AutoResetEvent(false);
		public ChaosInvocation Invocation { get; set; }
		public object Result { get; set; }
		public Exception Exception { get; set; }
		public Action Resolve { get; set; }
		public Action<Exception> Reject { get; set; }
		public bool Wait(TimeSpan waitTime)
		{
			return WaitEvent.WaitOne(waitTime);
		}
	}
}