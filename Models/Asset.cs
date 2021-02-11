namespace Smartfinance_server.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string CreationDate { get; set; }
        public string AcquiredDate { get; set; }
        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
        public int TransactionId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}