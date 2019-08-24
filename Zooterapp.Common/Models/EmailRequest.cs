using System.ComponentModel.DataAnnotations;

namespace Zooterapp.Common.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
