using System.ComponentModel.DataAnnotations;

namespace FarzamTEWebsite.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is Required!")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is Required!")]
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set;}
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string Role { get; set; }
        public string? Token { get; set; }
    }
}
