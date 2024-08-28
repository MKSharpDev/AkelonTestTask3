using AkelonTestTask3.Interface;

namespace AkelonTestTask3.Services
{
    public class AppService
    {
        private readonly IRepository _repository;

        public AppService(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("1 - получить по наименованию товара информацию о клиентах, " +
                    "заказавших этот товар, с указанием информации по количеству товара, цене и дате заказа.");
                Console.WriteLine("2 - Запрос на изменение контактного лица клиента с указанием параметров:" +
                    " Название организации, ФИО нового контактного лица");
                Console.WriteLine("3 - Запрос на определение золотого клиента");
                Console.WriteLine("4 - получить клиента по Id");
                Console.WriteLine("5 - выход ");
                Console.WriteLine();

                Console.WriteLine("Введите номер команды");
                try
                {
                    var command = int.Parse(Console.ReadLine());
                    switch (command)
                    {
                        case 1:
                            Console.WriteLine("Введите название товара");
                            var productName = Console.ReadLine();

                            var rez = _repository.GetProductOrdersInfo(productName);
                            if (rez.Any())
                            {
                                foreach (var response in rez)
                                {
                                    Console.WriteLine($"{response.OrganizationName} | {response.Address} |  " +
                                        $" {response.ContactPerson} | {response.ProductAmount} | {response.Price} | {response.OrderDate}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"не найдено заказоп пот товару - {productName}");
                            }
                            Console.WriteLine();

                            break;
                        case 2:
                            Console.WriteLine("Введите Id организации");
                            int customerId = int.Parse(Console.ReadLine());
                            var customer = _repository.GetCustomerById(customerId);
                            if (customer != null && customer.Id != 0)
                            {
                                Console.WriteLine("Организация:");
                                Console.WriteLine($"{customer.Id} {customer.OrganizationName} {customer.Address} {customer.ContactPerson}");

                                Console.WriteLine("Введите новое название организации:");
                                customer.OrganizationName = Console.ReadLine();
                                Console.WriteLine("Введите новое котакотное лицо:");
                                customer.ContactPerson = Console.ReadLine();
                                _repository.UpdateCusomer(customer);

                                var result = _repository.GetCustomerById(customer.Id);
                                Console.WriteLine("Организация изменена на:");
                                Console.WriteLine($"{customer.Id} {customer.OrganizationName} {customer.Address} {customer.ContactPerson}");
                            }
                            else
                            {
                                Console.WriteLine("Клиент с такий Id не найден");
                            }

                            Console.WriteLine();
                            break;
                        case 3:
                            Console.WriteLine("Введите начало периода");
                            DateTime periodStart = DateTime.Parse(Console.ReadLine());

                            Console.WriteLine("Введите конец периода");
                            DateTime periodEnd = DateTime.Parse(Console.ReadLine());



                            var id = _repository.GetGoldGustomer(periodStart, periodEnd);
                            if (id == null)
                            {
                                Console.WriteLine("Нет закзов за данный период");
                                Console.WriteLine("Проверьте период");
                                break;
                            }
                            var goldCustomer = _repository.GetCustomerById(id.Value);
                            Console.WriteLine("Золотой клиент:");
                            Console.WriteLine($"{goldCustomer.Id} {goldCustomer.OrganizationName} {goldCustomer.Address} {goldCustomer.ContactPerson}");

                            break;
                        case 4:
                            Console.WriteLine("Введите Id организации");
                            int getGustomerId = int.Parse(Console.ReadLine());
                            var getCustomer = _repository.GetCustomerById(getGustomerId);
                            if (getCustomer != null && getCustomer.Id != 0)
                            {
                                Console.WriteLine("Организация:");
                                Console.WriteLine($"{getCustomer.Id} {getCustomer.OrganizationName} {getCustomer.Address} {getCustomer.ContactPerson}");
                            }
                            else
                            {
                                Console.WriteLine("Клиент с такий Id не найден");
                            }

                            Console.WriteLine();
                            break;
                        case 5:
                            Console.WriteLine("Хотите закрыть программу? Введите y");
                            var cancel = Console.ReadLine();
                            if (cancel.ToLower() == "y")
                            {
                                exit = true;
                            }
                            else
                            {
                                Console.WriteLine("y не был введен, программа работает дальше ");
                            }

                            Console.WriteLine();
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
