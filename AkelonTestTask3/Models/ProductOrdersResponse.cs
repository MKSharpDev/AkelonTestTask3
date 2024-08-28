namespace AkelonTestTask3.Models
{
    public class ProductOrdersResponse
    {
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public int ProductAmount { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
