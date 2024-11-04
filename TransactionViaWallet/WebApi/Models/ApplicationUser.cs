using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int BankId {  get; set; }
        public int CIN { get; set; }
        public int BankCode { get; set; }
        public string Status { get; set; }
        public DateTime InscriptionDate { get; set; }
      
        public string Role { get; set; }
    } 
}
