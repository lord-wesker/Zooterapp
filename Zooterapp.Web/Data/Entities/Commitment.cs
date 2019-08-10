using System;

namespace Zooterapp.Web.Data.Entities
{
    public class Commitment
    {
        public int Id { get; set; }

        public string Remarks { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public Pet Pet { get; set; }

        public PetOwner PetOwner { get; set; }

        public Customer Customer { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public DateTime EndDateLocal => EndDate.ToLocalTime();

    }
}
