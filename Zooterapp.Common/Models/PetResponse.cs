using System.Collections.Generic;

namespace Zooterapp.Common.Models
{
    public class PetResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string PetType { get; set; }

        public string Race { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<PetImageResponse> PetImages { get; set; }

        public ICollection<CommitmentResponse> Commitments { get; set; }

        public ICollection<PetAchievementResponse> PetAchievements { get; set; }

    }
}
