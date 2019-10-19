namespace Zooterapp.Common.Models
{
    public class PetImageRequest
    {
        public int Id { get; set; }

        public int PetId { get; set; }

        public byte[] ImageArray { get; set; }
    }
}
