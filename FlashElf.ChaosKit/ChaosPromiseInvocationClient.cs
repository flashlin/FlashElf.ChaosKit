using System;
using System.Reactive.Subjects;
using Microsoft.Extensions.Options;

namespace FlashElf.ChaosKit
{
	public class ChaosPromiseInvocationClient : IChaosPromiseInvocationClient
	{
		private readonly IChaosClient _chaosClient;
		private readonly Subject<PromiseInvocation> _subjectActions = new Subject<PromiseInvocation>();
		private readonly ChaosClientConfig _config;

		public ChaosPromiseInvocationClient(IChaosClient chaosClient, IOptions<ChaosClientConfig> clientConfig)
		{
			_chaosClient = chaosClient;
			_config = clientConfig.Value;
			_subjectActions.Subscribe(this.ProcessAction);
		}

		public object Call(ChaosInvocation chaosInvocation)
		{
			var promise = new PromiseInvocation()
			{
				Invocation = chaosInvocation
			};
			
			Publish(promise);

			if (!promise.Wait(_config.WaitTimeout))
			{
				throw new TimeoutException();
			}

			if (promise.Exception != null)
			{
				throw promise.Exception;
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
				promiseInvocation.Exception = ex;
				if (promiseInvocation.Reject != null) { 
					promiseInvocation.Reject(ex);
				}
				promiseInvocation.WaitEvent.Set();
			}
		}
	}
}