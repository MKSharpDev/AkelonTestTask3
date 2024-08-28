using AkelonTestTask3.Models;

namespace AkelonTestTask3.Interface
{
    public interface IRepository
    {
        public List<ProductOrdersResponse> GetProductOrdersInfo(string productName);
        public Customer GetCustomerById(int id);
        public void UpdateCusomer(Customer customer);
        public int? GetGoldGustomer(DateTime periodStart, DateTime periodEnd);
    }
}