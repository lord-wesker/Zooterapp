using System;
using Prism.Commands;
using Prism.Navigation;
using Zooterapp.Common.Models;

namespace Zooterap.Prism.ViewModels
{
    public class CommitmentItemViewModel : CommitmentResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCommitmentCommand;

        public CommitmentItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectCommitmentCommand => _selectCommitmentCommand ?? (_selectCommitmentCommand = new DelegateCommand(SelectCommitment));

        private async void SelectCommitment()
        {
            var parameters = new NavigationParameters
            {
                { "Commitment", this }
            };

            await _navigationService.NavigateAsync("Commitments", parameters);
        }
    }
}
