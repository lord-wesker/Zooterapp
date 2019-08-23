using System.ComponentModel.DataAnnotations;

namespace Zooterapp.Web.Data.Entities
{
    public class PetImage
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public Pet Pet { get; set; }

        // TODO: CHANGE PROJECT PATH
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? "https://TBD.azurewebsites.net/images/Properties/noImage.png"
            : $"https://TBD.azurewebsites.net/{ImageUrl.Substring(1)}";

    }
}
