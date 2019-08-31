using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zooterapp.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboPetType();

        IEnumerable<SelectListItem> GetComboCustomers();

        IEnumerable<SelectListItem> GetComboAchievements();

        IEnumerable<SelectListItem> GetComboPets();
    }
}
