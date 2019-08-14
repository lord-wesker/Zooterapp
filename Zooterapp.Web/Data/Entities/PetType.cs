using System.Collections.Generic;

namespace Zooterapp.Web.Data.Entities
{
    public class PetType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}
