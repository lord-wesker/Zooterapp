using System.Collections.Generic;

namespace Zooterapp.Web.Data.Entities
{
    public class Achievement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<PetAchievement> PetAchievements { get; set; }
    }
}
