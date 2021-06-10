namespace Smartfinance_server.Models
{
    public class Liability
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public string CreationDate { get; set; }
        public string ContractDate { get; set; }
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
        public decimal Interest { get; set; }
        public decimal Annuity { get; set; }
        public string MaturityDate { get; set; }
        public string RefinancingDate { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Counterparty { get; set; }
        public string AssetIds { get; set; }
        public string TransactionIds { get; set; }
    }
}