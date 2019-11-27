using System.Threading.Tasks;

namespace CmdCoffee.Client
{
    public interface ICmdCoffeeApi
    {
        Task<dynamic> GetProducts();
        Task<string> PostOrder(string productCode, dynamic address, string promoCode);
    }
}