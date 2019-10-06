using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class CommitmentsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PetResponse _pet;
        private ObservableCollection<CommitmentItemViewModel> _commitments;

        public CommitmentsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Commitments";
        }

        public ObservableCollection<CommitmentItemViewModel> Commitments
        {
            get => _commitments;
            set => SetProperty(ref _commitments, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Pet"))
            {
                _pet = parameters.GetValue<PetResponse>("Pet");
                LoadCommitments();
            }
        }

        private void LoadCommitments()
        {
            Commitments = new ObservableCollection<CommitmentItemViewModel>(_pet.Commitments.Select(c => new CommitmentItemViewModel(_navigationService)
            {
                Customer = c.Customer,
                EndDate = c.EndDate,
                Id = c.Id,
                IsActive = c.IsActive,
                Price = c.Price,
                Remarks = c.Remarks,
                StartDate = c.StartDate,
            }));
        }
    }
}
