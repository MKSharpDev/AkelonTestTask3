namespace AkelonTestTask3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int OrderNumber {  get; set; }
        public int ProductAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
