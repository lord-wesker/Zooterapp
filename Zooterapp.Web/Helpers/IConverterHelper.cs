using System.Threading.Tasks;
using Zooterapp.Web.Data.Entities;
using Zooterapp.Web.Models;

namespace Zooterapp.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Commitment> ToCommitmentAsync(CommitmentViewModel view, bool isNew);
        CommitmentViewModel ToCommitmentViewModel(Commitment commitment);
        //PetAchievementViewModel ToPetAchievement(PetAchievement petachievement);
        Task<PetAchievement> ToPetAchievementAsync(AchievementViewModel model, bool isNew);
        Task<Pet> ToPetAsync(PetViewModel model, bool isNew);
        PetViewModel ToPetViewModel(Pet pet);
    }
}