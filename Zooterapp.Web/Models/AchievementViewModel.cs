using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Models
{
    public class AchievementViewModel : PetAchievement
    {
        public IEnumerable<SelectListItem> Achievements { get; set; }
    }
}
