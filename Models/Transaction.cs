namespace Smartfinance_server.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string BookingDate { get; set; }
        public string ValueDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Counterparty { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Saldo { get; set; }
    }
}