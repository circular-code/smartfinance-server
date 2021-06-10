namespace Smartfinance_server.Models
{
    public class Cashflow
    {
        public uint UserId { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}