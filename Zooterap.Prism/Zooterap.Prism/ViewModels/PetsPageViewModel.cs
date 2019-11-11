using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using Zooterapp.Common.Helpers;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PetOwnerResponse _petOwner;
        private ObservableCollection<PetItemViewModel> _pets;
        private TokenResponse _token;
        public PetsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pets";
            _navigationService = navigationService;
            LoadPets();
        }

        public ObservableCollection<PetItemViewModel> Pets
        {
            get => _pets;
            set => SetProperty(ref _pets, value);
        }

        private void LoadPets()
        {
            _token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            _petOwner = JsonConvert.DeserializeObject<PetOwnerResponse>(Settings.Owner);


            Title = $"{_petOwner.FirstName}'s Pets";
            Pets = new ObservableCollection<PetItemViewModel>(_petOwner.Pets.Select(p => new PetItemViewModel(_navigationService)
            {
                Id = p.Id,
                Age = p.Age,
                Commitments = p.Commitments,
                IsAvailable = p.IsAvailable,
                Name = p.Name,
                PetAchievements = p.PetAchievements,
                PetImages = p.PetImages,
                PetType = p.PetType,
                Race = p.Race,
            }).ToList());
        }
    }
}
