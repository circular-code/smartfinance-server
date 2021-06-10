namespace Smartfinance_server.Models
{
    public class Asset
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public string CreationDate { get; set; }
        public string ContractDate { get; set; }
        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
        public uint PrimaryTransactionId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal CurrentQuantity { get; set; }
        public string LiabilityIds { get; set; }
        public string TransactionIds { get; set; }
    }
}