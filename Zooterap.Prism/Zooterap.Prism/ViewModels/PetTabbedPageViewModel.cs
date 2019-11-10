using Prism.Navigation;

namespace Zooterap.Prism.ViewModels
{
    public class PetTabbedPageViewModel : ViewModelBase
    {
        public PetTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pet";
        }
    }
}
