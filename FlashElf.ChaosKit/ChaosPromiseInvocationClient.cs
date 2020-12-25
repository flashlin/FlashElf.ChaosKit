using System;
using System.Reactive.Subjects;

namespace FlashElf.ChaosKit
{
	public class ChaosPromiseInvocationClient : IChaosPromiseInvocationClient
	{
		private readonly IChaosClient _chaosClient;
		private Subject<PromiseInvocation> _subjectActions = new Subject<PromiseInvocation>();

		public ChaosPromiseInvocationClient(IChaosClient chaosClient)
		{
			_chaosClient = chaosClient;
			_subjectActions.Subscribe(this.ProcessAction);
		}

		public object Call(ChaosInvocation chaosInvocation)
		{
			var promise = new PromiseInvocation()
			{
				Invocation = chaosInvocation
			};
			
			Publish(promise);

			if (!promise.Wait(TimeSpan.FromSeconds(30)))
			{
				throw new TimeoutException();
			}
			return promise.Result;
		}

		public void Publish(PromiseInvocation invocation)
		{
			_subjectActions.OnNext(invocation);
		}

		protected void ProcessAction(PromiseInvocation promiseInvocation)
		{
			try
			{
				var resp = _chaosClient.Send(promiseInvocation.Invocation);
				promiseInvocation.Result = resp;
				if( promiseInvocation.Resolve != null) { 
					promiseInvocation.Resolve();
				}
				promiseInvocation.WaitEvent.Set();
			}
			catch(Exception ex)
			{
				if (promiseInvocation.Reject != null) { 
					promiseInvocation.Reject(ex);
				}
			}
		}
	}
}