using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace CmdCoffee.Client
{
    public interface ICmdCoffeeApi
    {
        Task<object> GetProducts();
    }

    public class CmdCoffeeApi : ICmdCoffeeApi
    {
        private const string CmdCoffeeApiAddress = "http://api.cmd.coffee";

        public async Task<dynamic> GetProducts()
        {
            return await CmdCoffeeApiAddress.AppendPathSegment("products").GetJsonListAsync();
        }
    }
}