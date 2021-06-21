using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Training.Models.V2
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);// 以非同步作業方式將 HTTP 要求傳送到內部處理常式，以傳送到伺服器

            int count = 0;
            while (count < MaxRetries && (response == null || !response.IsSuccessStatusCode))
            {
                count++;
                response = await base.SendAsync(request, cancellationToken);
                if (response != null && response.IsSuccessStatusCode)
                {
                    return response;
                }
            }
            return response;
        }
    }
}
