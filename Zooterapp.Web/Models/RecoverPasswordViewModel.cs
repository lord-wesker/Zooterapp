using System.ComponentModel.DataAnnotations;

namespace Zooterapp.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
