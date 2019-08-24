using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Models
{
    public class PetAchievementViewModel : PetAchievement
    {
        public int PetOwnerId { get; set; }

        [Display(Name = "Pet Achievement")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a pet achievement.")]
        public int PetAchievementID { get; set; }

        public IEnumerable<SelectListItem> PetAchievements { get; set; }
    }
}
