using AkelonTestTask3.Interface;
using AkelonTestTask3.Models;
using ClosedXML.Excel;

namespace AkelonTestTask3.ExcelContext
{
    public class ExelRepository : IRepository
    {
        private readonly string _filePath;
        public ExelRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<ProductOrdersResponse> GetProductOrdersInfo(string productName)
        {
            var result = new List<ProductOrdersResponse>();

            using (var workbook = new XLWorkbook(_filePath))
            {
                var productWorkSheet = workbook.Worksheet(1);
                var customerWorkSheet = workbook.Worksheet(2);
                var ordersWorkSheet = workbook.Worksheet(3);


                var productRowCount = productWorkSheet.RowsUsed().Count();

                bool productExist = false;
                int productId = 0;
                int price = 0;

                //проходим по таблице товаров, для поиска Id товара
                for (int i = 2; i <= productRowCount; i++)
                {
                    var value = productWorkSheet.Cell(i, 2).Value.ToString();
                    if (value == productName)
                    {
                        productId = int.Parse(productWorkSheet.Cell(i, 1).Value.ToString());
                        price = int.Parse(productWorkSheet.Cell(i, 4).Value.ToString());
                        productExist = true;
                        break;
                    }
                }

                if (!productExist)
                {
                    throw new Exception("не найден продукт");
                }
                else
                {
                    var ordersRowCount = ordersWorkSheet.RowsUsed().Count();
                    var ordersList = new List<Order>();

                    //в таблице заявок находим все заявки с искомым Id товара 
                    for (int i = 2; i <= ordersRowCount; i++)
                    {
                        var value = int.Parse(ordersWorkSheet.Cell(i, 2).Value.ToString());
                        if (value == productId)
                        {
                            ordersList.Add(new Order
                            {
                                Id = int.Parse(ordersWorkSheet.Cell(i, 1).Value.ToString()),
                                ProductId = int.Parse(ordersWorkSheet.Cell(i, 2).Value.ToString()),
                                CustomerId = int.Parse(ordersWorkSheet.Cell(i, 3).Value.ToString()),
                                OrderNumber = int.Parse(ordersWorkSheet.Cell(i, 4).Value.ToString()),
                                ProductAmount = int.Parse(ordersWorkSheet.Cell(i, 5).Value.ToString()),
                                OrderDate = DateTime.Parse(ordersWorkSheet.Cell(i, 6).Value.ToString())

                            });
                        }
                    }


                    var customerRowCount = customerWorkSheet.RowsUsed().Count();
                    //для каждого найденного заказа в таблице клиентов находим интересующего
                    foreach (var order in ordersList)
                    {

                        for (int i = 2; i <= customerRowCount; i++)
                        {
                            var value = int.Parse(customerWorkSheet.Cell(i, 1).Value.ToString());
                            if (value == order.CustomerId)
                            {
                                result.Add(new ProductOrdersResponse
                                {
                                    OrganizationName = customerWorkSheet.Cell(i, 2).Value.ToString(),
                                    Address = customerWorkSheet.Cell(i, 3).Value.ToString(),
                                    ContactPerson = customerWorkSheet.Cell(i, 4).Value.ToString(),
                                    ProductAmount = order.ProductAmount,
                                    Price = price,
                                    OrderDate = order.OrderDate
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Customer GetCustomerById(int id)
        {
            var customer = new Customer();
            using (var workbook = new XLWorkbook(_filePath))
            {
                var customerWorkSheet = workbook.Worksheet(2);
                var customerRowCount = customerWorkSheet.RowsUsed().Count();

                for (int i = 2; i <= customerRowCount; i++)
                {
                    var value = int.Parse(customerWorkSheet.Cell(i, 1).Value.ToString());
                    if (value == id)
                    {
                        customer.Id = value;
                        customer.OrganizationName = customerWorkSheet.Cell(i, 2).Value.ToString();
                        customer.Address = customerWorkSheet.Cell(i, 3).Value.ToString();
                        customer.ContactPerson = customerWorkSheet.Cell(i, 4).Value.ToString();
                    };
                }
            }
            return customer;
        }

        public void UpdateCusomer(Customer customer) 
        {
            using (var workbook = new XLWorkbook(_filePath))
            {
                var customerWorkSheet = workbook.Worksheet(2);
                var customerRowCount = customerWorkSheet.RowsUsed().Count();

                for (int i = 2; i <= customerRowCount; i++)
                {
                    var value = int.Parse(customerWorkSheet.Cell(i, 1).Value.ToString());
                    if (value == customer.Id)
                    {
                        customerWorkSheet.Cell(i, 2).Value = customer.OrganizationName;
                        customerWorkSheet.Cell(i, 4).Value = customer.ContactPerson;
                        workbook.Save();
                    };
                }
            }
        }

        public int? GetGoldGustomer(DateTime periodStart, DateTime periodEnd)
        {
            int? id = null;

            using (var workbook = new XLWorkbook(_filePath))
            {
                var ordersWorkSheet = workbook.Worksheet(3);
                var ordersRowCount = ordersWorkSheet.RowsUsed().Count();
                var ordersList = new List<Order>();
                
                var customerIdOrdersCountDict = new Dictionary<int, int>();

                for (int i = 2; i <= ordersRowCount; i++)
                {
                    var orderDate = DateTime.Parse(ordersWorkSheet.Cell(i, 6).Value.ToString());

                    if (periodStart <= orderDate && orderDate <= periodEnd)
                    {
                        var value = int.Parse(ordersWorkSheet.Cell(i, 2).Value.ToString());
                        var CustomerId = int.Parse(ordersWorkSheet.Cell(i, 3).Value.ToString());
                        var ProductAmount = int.Parse(ordersWorkSheet.Cell(i, 5).Value.ToString());

                        if (customerIdOrdersCountDict.ContainsKey(CustomerId))
                        {
                            customerIdOrdersCountDict[CustomerId] += ProductAmount;
                        }
                        else
                        {
                            customerIdOrdersCountDict.Add(CustomerId, ProductAmount);
                        }
                    }
                }
                if (customerIdOrdersCountDict == null || customerIdOrdersCountDict.Count == 0)
                {
                    return id;
                }
                else
                {
                    id = customerIdOrdersCountDict.First(x => x.Value == customerIdOrdersCountDict.Values.Max()).Key;
                }
            }
            return id;
        }
    }
}
