using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Models
{
    public class CommitmentViewModel : Commitment
    {
        public int PetOwnerId { get; set; }

        public int PetId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Customer")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Customer.")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

    }
}
