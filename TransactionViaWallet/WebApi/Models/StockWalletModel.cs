using System.ComponentModel.DataAnnotations;
namespace WebAPI.Models
{
    public class StockWalletModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "BankCode is required")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "BankCode must be a 3 digit number")]
        public int BankCode { get; set; }

        [Required(ErrorMessage = "Stock is required")]

        public int Stock { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(@"^(libre|reserved)$", ErrorMessage = "Status must be either 'libre' or 'reserved'")]
        public string Status { get; set; }
    }
}