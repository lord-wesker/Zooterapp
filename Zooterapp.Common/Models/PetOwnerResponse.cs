using System.Collections.Generic;

namespace Zooterapp.Common.Models
{
    public class PetOwnerResponse
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Document { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ICollection<PetResponse> Pets { get; set; }

        public ICollection<CommitmentResponse> Commitments { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

}
