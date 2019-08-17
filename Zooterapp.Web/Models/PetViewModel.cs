using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Models
{
    public class PetViewModel: Pet
    {
        public int PetOwnerId { get; set; }

        [Display(Name = "Pet Type")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a pet type.")]
        public int PetTypeID { get; set; }

        public IEnumerable<SelectListItem> PetTypes { get; set; }
    }

}
