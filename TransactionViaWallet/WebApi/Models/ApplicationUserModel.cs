using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace WebAPI.Models
{
    public class ApplicationUserModel 
    {
        [Key]
        [Column(Order = 1)]
        public int BankId { get; set; }
        public string Id { get; set; }

        [Key]
        [Column(Order = 2)]
        public int PhoneNumber { get; set; }

        [Key]
        [Column(Order = 3)]
        public int CIN { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)] // Exemple de longueur maximale pour le mot de passe
        public string Password { get; set; }

        [Required]
        [MaxLength(50)] // Exemple de longueur maximale pour le nom d'utilisateur
        public string UserName { get; set; }

        [MaxLength(50)] // Exemple de longueur maximale pour le rôle
        public string Role { get; set; }

        public int BankCode { get; set; }

        [MaxLength(50)] // Exemple de longueur maximale pour le statut
        public string Status { get; set; }

        public DateTime InscriptionDate { get; set; }
    }
}
