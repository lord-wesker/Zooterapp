﻿using System;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Zooterapp.Common.Helpers;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class PetItemViewModel : PetResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectPetCommand;


        public PetItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public DelegateCommand SelectPetCommand => _selectPetCommand ?? (_selectPetCommand = new DelegateCommand(SelectPet));

        private async void SelectPet()
        {
            //var parameters = new NavigationParameters
            //{
            //    { "Pet", this }
            //};

            Settings.Pet = JsonConvert.SerializeObject(this);

            await _navigationService.NavigateAsync("PetTabbedPage");
        }
    }
}
