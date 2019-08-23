using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Models
{
    public class PetImageViewModel : PetImage
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }

}
