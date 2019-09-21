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
            ? "https://zooterappweb.azurewebsites.net/images/Pets/noImage.png"
            : $"https://zooterappweb.azurewebsites.net/{ImageUrl.Substring(1)}";

    }
}
