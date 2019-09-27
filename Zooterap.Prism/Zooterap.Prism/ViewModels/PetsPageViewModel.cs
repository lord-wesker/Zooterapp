using Prism.Mvvm;
using Prism.Navigation;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private PetOwnerResponse _petowner;
        private readonly INavigationService _navigationService;
        public PetsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Pets";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("petowner"))
            {
                _petowner = parameters.GetValue<PetOwnerResponse>("petowner");
                Title = _petowner.FullName;
            }
        }

    }
}
