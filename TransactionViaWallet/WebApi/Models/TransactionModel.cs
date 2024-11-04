namespace WebApi.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
       public DateTime Date { get; set; } 
        public string DestinationWalletNum { get; set; } 
    }
}
