using System.Threading.Tasks;

namespace CmdCoffee.Client
{
    public interface ICmdCoffeeApi
    {
        Task<dynamic> GetProducts();
        Task<dynamic> PostOrder(string productCode, dynamic address, string promoCode);
        Task<dynamic> GetOrders();
        Task<dynamic> GetOrder(string orderKey);
    }
}