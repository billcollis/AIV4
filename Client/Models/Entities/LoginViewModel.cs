using System.ComponentModel.DataAnnotations;

namespace AIV4.Client.Models.Entities
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage ="Please provide User name")]
        public string? Username { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your password")]
        public string? Password { get; set; } = String.Empty;

    }
}
