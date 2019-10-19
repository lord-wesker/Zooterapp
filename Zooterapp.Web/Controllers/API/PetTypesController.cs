using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetTypesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PetTypesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IEnumerable<PetType> GetPropertyTypes()
        {
            return _dataContext.PetTypes.OrderBy(pt => pt.Name);
        }

    }
}