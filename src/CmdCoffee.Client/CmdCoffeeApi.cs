using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

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

        public async Task<string> PostOrder(string productCode, dynamic address, string promoCode)
        {

            var httpResponseMessage = await GetBaseRequest("orders")
                .PostJsonAsync(new {productCode = productCode, shippingAddress = address, promoCode = promoCode});

            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }

            return result;

        }

        private IFlurlRequest GetBaseRequest(string endpoint)
        {
            return _apiSettings.CmdCoffeeApiAddress.AppendPathSegment(endpoint)
                .WithHeader("X-Access-Key", _apiSettings.AccessKey)
                .AllowAnyHttpStatus();
        }
    }
}