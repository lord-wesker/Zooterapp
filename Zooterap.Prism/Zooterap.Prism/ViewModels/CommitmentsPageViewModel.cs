using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using Zooterapp.Common.Models;
using Newtonsoft.Json;
using Zooterapp.Common.Helpers;

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

            Pet = JsonConvert.DeserializeObject<PetResponse>(Settings.Pet);
            LoadCommitments();
        }

        public ObservableCollection<CommitmentItemViewModel> Commitments
        {
            get => _commitments;
            set => SetProperty(ref _commitments, value);
        }

        //public override void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    base.OnNavigatedTo(parameters);

        //    if (parameters.ContainsKey("Pet"))
        //    {
        //        _pet = parameters.GetValue<PetResponse>("Pet");
        //        LoadCommitments();
        //    }
        //}

        public PetResponse Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
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
