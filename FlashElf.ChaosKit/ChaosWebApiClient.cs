using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using T1.Standard.Threads;
using T1.Standard.Web;

namespace FlashElf.ChaosKit
{
    /// <summary>
    /// 
    /// </summary>
    /// <example><![CDATA[
    /// [Route("api/[controller]/[action]")]
    /// [ApiController]
    /// public class ChaosController : ControllerBase
    /// {
    ///     IChaosService _chaosService;
    ///     [HttpPost]
    ///     public ChaosInvocationResp Send(ChaosInvocationReq req){
    ///         return _chaosService.ProcessInvocation(req);
    ///     }
    /// }
    /// ]]>
    /// </example>
    public class ChaosWebApiClient : IChaosClient
    {
        private readonly IWebApiClient _webApiClient;
        private readonly ChaosWebApiClientConfig _config;
        private readonly IChaosConverter _chaosConverter;

        public ChaosWebApiClient(
            IOptions<ChaosWebApiClientConfig> config,
            IWebApiClient webApiClient,
            IChaosConverter chaosConverter)
        {
            _chaosConverter = chaosConverter;
            _config = config.Value;
            _webApiClient = webApiClient;
        }

        public object Send(ChaosInvocation invocation)
        {
            return AsyncHelper.RunSync(async () =>
            {
                var reply = await _webApiClient.PostJsonAsync<ChaosInvocationResp>($"{_config.Uri}/api/Chaos/Send",
                    new Dictionary<string, string>(), invocation);
                return _chaosConverter.ToData(reply);
            });
        }
    }
}