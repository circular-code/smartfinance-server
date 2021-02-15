namespace Smartfinance_server.Models
{
    public class Liability
    {
        public string user { get; set; }
        public int Id { get; set; }
        public string CreationDate { get; set; }
        public string acquiredDate { get; set; }
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Counterparty { get; set; }
    }
}