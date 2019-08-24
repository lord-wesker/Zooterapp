using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Common.Models;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetOwnersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PetOwnersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetPetOwnerByEmail")]
        public async Task<IActionResult> GetOwnerByEmailAsync(EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var petOwner = await _dataContext.PetOwners
                .Include(o => o.User)
                .Include(o => o.Pets)
                .ThenInclude(p => p.PetType)
                .Include(o => o.Pets)
                .ThenInclude(p => p.PetImages)
                .Include(o => o.Commitments)
                .ThenInclude(c => c.Customer)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(o => o.User.Email.ToLower() == request.Email.ToLower());

            if (petOwner == null)
            {
                return NotFound();
            }

            var response = new PetOwnerResponse
            {
                Id = petOwner.Id,
                FirstName = petOwner.User.Name,
                LastName = petOwner.User.LastName,
                Address = petOwner.User.Address,
                Document = petOwner.User.Document,
                Email = petOwner.User.Email,
                PhoneNumber = petOwner.User.PhoneNumber,
                Pets = petOwner.Pets?.Select(p => new PetResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Age = p.Age,
                    Race = p.Race,
                    PetType = p.PetType.Name,
                    Commitments = p.Commitments?.Select(c => new CommitmentResponse
                    {
                        Id = c.Id,
                        Customer = ToCustomerResponse(c.Customer),
                        IsActive = c.IsActive,
                        Price = c.Price,
                        Remarks = c.Remarks,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                    }).ToList(),
                    IsAvailable = p.IsAvailable,
                    PetAchievements = p.PetAchievements?.Select(pa => new PetAchievementResponse
                    {
                        AchievementId = pa.Id,
                        Achievement = pa.Achievement.Name,
                    }).ToList(),
                    PetImages = p.PetImages?.Select(pi => new PetImageResponse
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                }).ToList()
            };

            return Ok(response);
        }

        private CustomerResponse ToCustomerResponse(Customer lessee)
        {
            return new CustomerResponse
            {
                Id = lessee.Id,
                Address = lessee.User.Address,
                Document = lessee.User.Document,
                Email = lessee.User.Email,
                FirstName = lessee.User.Name,
                LastName = lessee.User.LastName,
                PhoneNumber = lessee.User.PhoneNumber
            };
        }
    }
}
