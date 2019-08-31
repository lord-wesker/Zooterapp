using System.ComponentModel.DataAnnotations;

namespace Zooterapp.Web.Data.Entities
{
    public class PetAchievement
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }

        [Display(Name = "Pet Achievement")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a pet achievement.")]
        public int AchievementId { get; set; }

        public Achievement Achievement { get; set; }
    }
}
