using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class ZooterappMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ZooterappMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_pets",
                    PageName = "PetsPage",
                    Title = "Pets"
                },

                new Menu
                {
                    Icon = "ic_book",
                    PageName = "CommitmentsPage",
                    Title = "Commitments"
                },

                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ModifyUserPage",
                    Title = "Modify User"
                },

                new Menu
                {
                    Icon = "ic_map",
                    PageName = "MapPage",
                    Title = "Map"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = "Log out"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
