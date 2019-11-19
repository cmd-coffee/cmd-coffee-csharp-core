using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace CmdCoffee.Client
{
    public class CmdCoffeeApi
    {
        private const string CmdCoffeeApiAddress = "http://api.cmd.coffee";

        public static async Task<dynamic> GetProducts()
        {
            return await CmdCoffeeApiAddress.AppendPathSegment("products").GetJsonListAsync();
        }
    }
}