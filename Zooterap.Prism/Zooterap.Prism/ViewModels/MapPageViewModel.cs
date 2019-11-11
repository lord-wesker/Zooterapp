using Prism.Mvvm;
using Prism.Navigation;

namespace Zooterap.Prism.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        public MapPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Map";
        }
    }
}
