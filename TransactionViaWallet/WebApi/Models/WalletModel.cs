using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class WalletModel
    {
        [Key]
        public string NumWallet { get; set; }

        public string Validity { get; set; }

        public String WalletStatus { get; set; }

        public decimal Solde { get; set; }

        [ForeignKey("ApplicationUser")]
        public int CIN { get; set; }

        //public ApplicationUserModel User { get; set; } 
    }
}
