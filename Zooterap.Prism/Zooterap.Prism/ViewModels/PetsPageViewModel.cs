using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PetOwnerResponse _petOwner;
        private ObservableCollection<PetItemViewModel> _pets;
        public PetsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pets";
            _navigationService = navigationService;
        }

        public ObservableCollection<PetItemViewModel> Pets
        {
            get => _pets;
            set => SetProperty(ref _pets, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("petOwner"))
            {
                _petOwner = parameters.GetValue<PetOwnerResponse>("petOwner");
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
}
