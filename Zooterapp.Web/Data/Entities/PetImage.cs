namespace Zooterapp.Web.Data.Entities
{
    public class PetImage
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public Pet Pet { get; set; }

        // TODO: CHANGE PROJECT PATH
        public string ImageFullPath => "";
    }
}
