using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Daybar.Core.Model.Messages;
using DayBar.Contract.Auth;
using XamlingCore.Portable.Contract.Config;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Messages.Network;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Net.DownloadConfig;
using XamlingCore.Portable.Net.Service;

namespace Daybar.Core.Model.Config
{
    public class TransferConfigService : HttpTransferConfigServiceBase
    {
        
        private readonly ITokenGetService _tokenGetService;
        private readonly IDeviceNetworkStatus _deviceNetworkStatus;

        private const string baseUrl = "";

        public TransferConfigService(ITokenGetService tokenGetService, IDeviceNetworkStatus deviceNetworkStatus)
        {
            _tokenGetService = tokenGetService;
            _deviceNetworkStatus = deviceNetworkStatus;
        }

        public override Task OnUnauthorizedResult(HttpResponseMessage result,
            IHttpTransferConfig originalConfig)
        {
            new TokenExpiredMessage().Send();
            return base.OnUnauthorizedResult(result, originalConfig);
        }

        protected override void OnDownloadException(Exception ex, string source, IHttpTransferConfig originalConfig)
        {
            new NoNetworkErrorMessage().Send();
            base.OnDownloadException(ex, source, originalConfig);
        }

        public override Task OnUnsuccessfulResult(HttpResponseMessage result, IHttpTransferConfig originalConfig)
        {
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                new NoNetworkErrorMessage().Send();
            }

            return base.OnUnsuccessfulResult(result, originalConfig);
        }

        public override async Task<IHttpTransferConfig> GetConfig(string url, string verb)
        {
            var config = new StandardHttpConfig
            {
                Accept = "application/json",
                IsValid = true,
                Url = url,
                BaseUrl = url,
                Verb = verb,
                Headers = new Dictionary<string, string>()
            };

            await _setupToken(config);

            return config;
        }

        async Task _setupToken(StandardHttpConfig config)
        {
            var token = await _tokenGetService.GetToken();

            if (token == null)
            {
                return;
            }

            config.Auth = token;
            config.AuthScheme = "Bearer";
        }
    }
}
