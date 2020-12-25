using System;
using System.Reactive.Subjects;

namespace FlashElf.ChaosKit
{
	public class ChaosPromiseInvocationSubjects : IChaosPromiseInvocationSubjects
	{
		private readonly IChaosClient _chaosClient;

		public ChaosPromiseInvocationSubjects(IChaosClient chaosClient)
		{
			_chaosClient = chaosClient;
			SubjectActions.Subscribe(this.ProcessAction);
		}

		public Subject<PromiseInvocation> SubjectActions = new Subject<PromiseInvocation>();

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
			SubjectActions.OnNext(invocation);
		}

		protected void ProcessAction(PromiseInvocation promiseInvocation)
		{
			try
			{
				var resp = _chaosClient.Send(promiseInvocation.Invocation);
				promiseInvocation.Result = resp;
				promiseInvocation?.Resolve();
				promiseInvocation.WaitEvent.Set();
			}
			catch(Exception ex)
			{
				promiseInvocation?.Reject(ex);
			}
		}
	}
}