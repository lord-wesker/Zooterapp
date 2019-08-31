using System.Collections.Generic;
using System.Threading.Tasks;
using Zooterapp.Web.Data;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Models;

namespace Zooterapp.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _context = dataContext;
            _combosHelper = combosHelper;
        }

        public PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                Id = pet.Id,
                Name = pet.Name,
                Race = pet.Race,
                Age = pet.Age,
                PetOwnerId = pet.Owner.Id,
                PetTypes = _combosHelper.GetComboPetType(),
                PetTypeID = pet.PetType.Id,
                IsAvailable = pet.IsAvailable,
            };
        }

        public async Task<Pet> ToPetAsync(PetViewModel model, bool isNew)
        {
            return new Pet
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Age = model.Age,
                Race = model.Race,
                PetType = await _context.PetTypes.FindAsync(model.PetTypeID),
                Owner = await _context.PetOwners.FindAsync(model.PetOwnerId),
                IsAvailable = model.IsAvailable,
                Commitments = isNew ? new List<Commitment>() : model.Commitments,
                PetImages = isNew ? new List<PetImage>() : model.PetImages,
                PetAchievements = isNew ? new List<PetAchievement>() : model.PetAchievements,
            };
        }

        public async Task<Commitment> ToCommitmentAsync(CommitmentViewModel view, bool isNew)
        {
            return new Commitment
            {
                Id = isNew ? 0 : view.Id,
                EndDate = view.EndDate,
                IsActive = view.IsActive,
                Customer = await _context.Customers.FindAsync(view.CustomerId),
                PetOwner = await _context.PetOwners.FindAsync(view.PetOwnerId),
                Price = view.Price,
                Pet = await _context.Pets.FindAsync(view.PetId),
                Remarks = view.Remarks,
                StartDate = view.StartDate,
            };
        }

        public CommitmentViewModel ToCommitmentViewModel(Commitment commitment)
        {
            return new CommitmentViewModel
            {
                Id = commitment.Id,
                Customer = commitment.Customer,
                CustomerId = commitment.Customer.Id,
                Customers = _combosHelper.GetComboCustomers(),
                Pet = commitment.Pet,
                PetId = commitment.Pet.Id,
                PetOwner = commitment.PetOwner,
                PetOwnerId = commitment.PetOwner.Id,
                Price = commitment.Price,
                Remarks = commitment.Remarks,
                IsActive = commitment.IsActive,
                StartDate = commitment.StartDate,
                EndDate = commitment.EndDate
            };
        }

        //public PetAchievementViewModel ToPetAchievement(PetAchievement petachievement)
        //{
        //    return new PetAchievementViewModel
        //    {
        //        Id = petachievement.Id,
        //        PetAchievements = _combosHelper.GetComboPetsAchievements(),
        //        PetAchievementID = petachievement.AchievementId,
        //        Pet = petachievement.Pet,
        //        PetId = petachievement.PetId,
        //    };
        //}


        //public async Task<PetAchievement> ToPetAchievementAsync(PetAchievementViewModel model, bool isNew)
        //{
        //    return new PetAchievement
        //    {
        //        Id = isNew ? 0 : model.PetAchievementID,
        //        Achievement = await _context.Achievements.FindAsync(model.AchievementId),
        //        Pet = await _context.Pets.FindAsync(model.PetId),
        //        PetId = model.PetId,
        //        AchievementId = model.AchievementId,
        //    };
        //}

        //public PetAchievementViewModel ToPetAchievementViewModel(PetAchievement petAchievement)
        //{
        //    return new PetAchievementViewModel
        //    {
        //        Id = petAchievement.Id,
        //        Achievement = petAchievement.Achievement,
        //        PetId = petAchievement.PetId
        //    };
        //}
    }
}
