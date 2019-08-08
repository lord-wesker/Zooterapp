namespace Zooterapp.Web.Data.Entities
{
    public class PetRate
    {
        public int Id { get; set; }

        public Pet Pet { get; set; }

        public int Rate { get; set; }

        public Commitment Commitment { get; set; }

        public string Comment { get; set; }
    }
}
