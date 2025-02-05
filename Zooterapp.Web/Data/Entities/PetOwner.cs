﻿using System.Collections.Generic;

namespace Zooterapp.Web.Data.Entities
{
    public class PetOwner
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<Pet> Pets { get; set; }

        public ICollection<Commitment> Commitments { get; set; }
    }
}
