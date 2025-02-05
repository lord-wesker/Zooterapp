﻿using Prism.Navigation;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class PetPageViewModel : ViewModelBase
    {
        private PetResponse _pet;

        public PetPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Pet Page";
        }

        public PetResponse Pet {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("Pet"))
            {
                Pet = parameters.GetValue<PetResponse>("Pet");
                Title = Pet.Name;
            }
        }
    }
}
