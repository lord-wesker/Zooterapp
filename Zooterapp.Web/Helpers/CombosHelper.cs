using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Web.Data;

namespace Zooterapp.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;
        
        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboCustomers()
        {
            var list = _dataContext.Customers.Select(l => new SelectListItem
            {
                Text = l.User.FullNameWithDocument,
                Value = $"{l.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a customer...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPetType()
        {
            var list = _dataContext.PetTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a pet type...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPets()
        {
            var list = _dataContext.PetTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = pt.Id.ToString()
            }).OrderBy(pt => pt.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Pet Type ...)",
                Value = "0",
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboAchievements()
        {
            var list = _dataContext.Achievements.Select(pa => new SelectListItem
            {
                Text = pa.Name,
                Value = pa.Id.ToString()
            }).OrderBy(pt => pt.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Pet Achievement ...)",
                Value = "0",
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboAchievements()
        {
            var list = _dataContext.Achievements.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).OrderBy(a => a.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an Achievement ...)",
                Value = "0",
            });

            return list;
        }
    }
}
