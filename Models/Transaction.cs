namespace Smartfinance_server.Models
{
    public class Transaction
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public string BookingDate { get; set; }
        public string ValueDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Counterparty { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Saldo { get; set; }

        public Transaction()
        {
            this.Id = 0;
            this.UserId = 0;
            this.BookingDate = "";
            this.ValueDate = "";
            this.Amount = 0;
            this.Currency = "";
            this.Counterparty = "";
            this.Type = "";
            this.Description = "";
            this.Saldo = 0;
        }
    }
}