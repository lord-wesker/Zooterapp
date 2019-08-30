using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zooterapp.Web.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public PetType PetType { get; set; }

        public string Race { get; set; }

        public PetOwner Owner { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<PetImage> PetImages { get; set; }

        public ICollection<Commitment> Commitments { get; set; }

        [Display(Name="Pet Achievements")]
        public ICollection<PetAchievement> PetAchievements { get; set; }
    }
}
