using System;
using System.Linq.Expressions;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using T1.Standard.Common;
using T1.Standard.DynamicCode;

namespace FlashElf.ChaosKit
{
	public class ChaosPromiseInvocationClient : IChaosPromiseInvocationClient
	{
		private readonly IChaosClient _chaosClient;
		private readonly Subject<PromiseInvocation> _subjectActions = new Subject<PromiseInvocation>();
		private readonly ChaosClientConfig _config;
		private readonly TypeFinder _typeFinder;

		public ChaosPromiseInvocationClient(IChaosClient chaosClient, IOptions<ChaosClientConfig> clientConfig)
		{
			_chaosClient = chaosClient;
			_config = clientConfig.Value;
			_subjectActions.Subscribe(this.ProcessAction);
			_typeFinder = new TypeFinder();
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
				if (promiseInvocation.Invocation.IsReturnTask)
				{
					var returnType = _typeFinder.Find(promiseInvocation.Invocation.ReturnTypeFullName);
					var taskFromResult = DynamicMethod.GetTaskFromResult(returnType);
					promiseInvocation.Result = taskFromResult(resp);
				}

				promiseInvocation.Resolve?.Invoke();
				promiseInvocation.WaitEvent.Set();
			}
			catch(Exception ex)
			{
				promiseInvocation.Exception = ex;
				promiseInvocation.Reject?.Invoke(ex);
				promiseInvocation.WaitEvent.Set();
			}
		}
	}
}