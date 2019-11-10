using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zooterapp.Common.Helpers;
using Zooterapp.Common.Models;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;

namespace Zooterapp.Web.Controllers.API
{
    // TODO: COMPLETE REQUESTS PROPERTIES
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PetsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostProperty([FromBody] PetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var owner = await _dataContext.PetOwners.FindAsync(request.PetOwnerId);
            if (owner == null)
            {
                return BadRequest("Not valid owner.");
            }

            var propertyType = await _dataContext.PetTypes.FindAsync(request.PetTypeId);
            if (propertyType == null)
            {
                return BadRequest("Not valid property type.");
            }

            var pet = new Pet
            {
                IsAvailable = request.IsAvailable,
                Owner = owner,
            };

            _dataContext.Pets.Add(pet);
            await _dataContext.SaveChangesAsync();
            return Ok(pet);
        }

        [HttpPost]
        [Route("AddImageToProperty")]
        public async Task<IActionResult> AddImageToProperty([FromBody] PetImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pet = await _dataContext.Pets.FindAsync(request.PetId);
            if (pet == null)
            {
                return BadRequest("Not valid property.");
            }

            var imageUrl = string.Empty;
            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Properties";
                var fullPath = $"~/images/Properties/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }

            var petImage = new PetImage
            {
                ImageUrl = imageUrl,
                Pet = pet
            };

            _dataContext.PetImages.Add(petImage);
            await _dataContext.SaveChangesAsync();
            return Ok(petImage);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutProperty([FromRoute] int id, [FromBody] PetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            var oldPet = await _dataContext.Pets.FindAsync(request.Id);
            if (oldPet == null)
            {
                return BadRequest("Property doesn't exists.");
            }

            var propertyType = await _dataContext.PetTypes.FindAsync(request.PetTypeId);
            if (propertyType == null)
            {
                return BadRequest("Not valid property type.");
            }

            oldPet.IsAvailable = request.IsAvailable;

            _dataContext.Pets.Update(oldPet);
            await _dataContext.SaveChangesAsync();
            return Ok(oldPet);
        }

        [HttpPost]
        [Route("DeleteImageToProperty")]
        public async Task<IActionResult> DeleteImageToProperty([FromBody] PetImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var petImage = await _dataContext.PetImages.FindAsync(request.Id);
            if (petImage == null)
            {
                return BadRequest("Property image doesn't exist.");
            }

            _dataContext.PetImages.Remove(petImage);
            await _dataContext.SaveChangesAsync();
            return Ok(petImage);
        }

    }
}