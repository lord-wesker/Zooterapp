using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private PetOwnerResponse _petOwner;
        private ObservableCollection<PetResponse> _pets;
        public PetsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pets";
        }

        public ObservableCollection<PetResponse> Pets
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
                Pets = new ObservableCollection<PetResponse>(_petOwner.Pets);
            }
        }
    }
}
