using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zooterapp.Common.Models;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Helpers;

namespace Zooterapp.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetOwnersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public PetOwnersController(DataContext dataContext, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetPetOwnerByEmail")]
        public async Task<IActionResult> GetPetOwnerByEmailAsync(EmailRequest request)
        {
            try
            {
                var user = await _userHelper.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (await _userHelper.IsUserInRoleAsync(user, "PetOwner"))
                {
                    return await GetPetOwnerAsync(request);
                }
                else
                {
                    return await GetCustomerAsync(request);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        private async Task<IActionResult> GetCustomerAsync(EmailRequest emailRequest)
        {
            var customer = await _dataContext.Customers
                .Include(o => o.User)
                .Include(o => o.Commitments)
                .ThenInclude(c => c.PetOwner)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(emailRequest.Email.ToLower()));

            var pets = await _dataContext.Pets
                .Include(p => p.PetType)
                .Include(p => p.PetImages)
                .Where(p => p.IsAvailable)
                .ToListAsync();

            var response = new PetOwnerResponse
            {
                RoleId = 2,
                Id = customer.Id,
                FirstName = customer.User.Name,
                LastName = customer.User.LastName,
                Address = customer.User.Address,
                Document = customer.User.Document,
                Email = customer.User.Email,
                PhoneNumber = customer.User.PhoneNumber,
                Pets = pets?.Select(p => new PetResponse
                {
                    Id = p.Id,
                    IsAvailable = p.IsAvailable,
                    PetImages = p.PetImages?.Select(pi => new PetImageResponse
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                    PetType = p.PetType.Name,
                    PetAchievements = p.PetAchievements?.Select(pa => new PetAchievementResponse {
                        AchievementId = pa.Id,
                        Achievement = pa.Achievement.Name
                    }).ToList(),
                    Age = p.Age,
                    Race = p.Race,
                    Name = p.Name,
                    
                }).ToList(),
                Commitments = customer.Commitments?.Select(c => new CommitmentResponse
                {
                    EndDate = c.EndDate,
                    Id = c.Id,
                    IsActive = c.IsActive,
                    Price = c.Price,
                    Remarks = c.Remarks,
                    StartDate = c.StartDate
                }).ToList()
            };

            return Ok(response);
        }

        private async Task<IActionResult> GetPetOwnerAsync(EmailRequest request)
        {
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
