using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarzamTEWebsite.Models
{
    [NotMapped]
    public class ChangePassword
    {
        [Required(ErrorMessage = "Password is Required!")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "New_Password is Required!")]
        public required string New_Password { get; set; }
    }
}
