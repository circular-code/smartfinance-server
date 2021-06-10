namespace Smartfinance_server.Models
{
    public class AssetHistoryEntry
    {
        public int UserId { get; set; }
        public int ReferenceId { get; set; }
        public string CreationDate { get; set; }
        public decimal Value { get; set; }
        public decimal BuyQuantity { get; set; }
        public decimal BuyPrice { get; set; }
    }
}