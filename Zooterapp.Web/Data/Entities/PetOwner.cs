using System.Collections.Generic;

namespace Zooterapp.Web.Data.Entities
{
    public class PetOwner
    {
        public int Id { get; set; }

        public string Document { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Address { get; set; }

        public string FullName => $"{Name} {LastName}";

        public string FullNameWithDocument => $"{FullName} - {Document}";

        public ICollection<Pet> Pets { get; set; }

        public ICollection<Commitment> Commitments { get; set; }
    }
}
