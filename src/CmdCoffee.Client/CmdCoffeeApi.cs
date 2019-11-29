using System;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace CmdCoffee.Client
{
    public class CmdCoffeeApi : ICmdCoffeeApi
    {
        private readonly ICmdCoffeeApiSettings _apiSettings;

        public CmdCoffeeApi(Func<ICmdCoffeeApiSettings> apiSettingsFactory)
        {
            _apiSettings = apiSettingsFactory();
        }

        public async Task<dynamic> GetProducts()
        {
            return await GetBaseRequest("products").GetJsonListAsync();
        }

        public async Task<dynamic> GetOrders()
        {
            return await GetBaseRequest("orders").GetJsonListAsync();
        }

        public async Task<dynamic> GetOrder(string orderKey)
        {
            return await GetBaseRequest($"orders/{orderKey}").GetJsonAsync();
       }

        public async Task<dynamic> Join(string inviteCode)
        {
            var httpResponseMessage = await GetBaseRequest("join")
                .PostJsonAsync(new { inviteCode = inviteCode});

            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            return JObject.Parse(result);
        }

        public async Task<dynamic> PostOrder(string productCode, dynamic address, string promoCode)
        {

            var httpResponseMessage = await GetBaseRequest("orders")
                .PostJsonAsync(new {productCode = productCode, shippingAddress = address, promoCode = promoCode});

            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized. Please verify your access-key.");
            }

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            return JObject.Parse(result);

        }

        private IFlurlRequest GetBaseRequest(string endpoint)
        {
            return _apiSettings.CmdCoffeeApiAddress.AppendPathSegment(endpoint)
                .WithHeader("X-Access-Key", _apiSettings.AccessKey)
                .AllowAnyHttpStatus();
        }
    }
}