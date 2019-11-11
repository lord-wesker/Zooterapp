using System.ComponentModel.DataAnnotations;

namespace Zooterapp.Common.Models
{
    public class PetRequest
    {
        public int Id { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public int PetTypeId { get; set; }

        [Required]
        public int PetOwnerId { get; set; }
    }

}
