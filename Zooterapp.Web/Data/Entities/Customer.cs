using System.Collections.Generic;

namespace Zooterapp.Web.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public User User { get; set; }
        public ICollection<Commitment> Commitments { get; set; }
    }
}
