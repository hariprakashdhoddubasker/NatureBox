using NatureBox.Commands;
using NatureBox.Partners.Service;
using NatureBox.Model;
using NatureBox.Transactions.Service;
using NatureBox.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NatureBox.Partners
{
    public class PartnerSettlementReportViewModel : BaseViewModel
    {
        private ObservableCollection<Partner> myGridPartners;
        private Partner myPartner;
        private string mySearchString;

        private List<Partner> myAllPartners;
        private List<PartnerPayment> myAllPartnerPayments;
        private ObservableCollection<PartnerPayment> myGridPartnerPayments;
        private DateTime myFromDate;
        private DateTime myToDate;
        private bool myIsAllFilter;
        private bool myIsAutoCompleteTextBoxEnabled;
        private readonly IPartnerPaymentRepository myPartnerPaymentRepo;
        private readonly IPartnerRepository myPartnerRepo;

        public PartnerSettlementReportViewModel(IPartnerPaymentRepository partnerPaymentRepo, IPartnerRepository partnerRepo)
        {
            this.myPartnerRepo = partnerRepo;
            this.myPartnerPaymentRepo = partnerPaymentRepo;
            this.myAllPartners = new List<Partner>();
            this.BtnSearchCommand = new Command(this.OnSearchButtonClick, this.CanExecuteSearch);
            this.GridPartnerPayments = new ObservableCollection<PartnerPayment>();
            this.IsAllFilter = false;
        }

        public ICommand BtnSearchCommand { get; private set; }

        public Partner Partner
        {
            get => this.myPartner;
            set
            {
                SetProperty(ref myPartner, value);
                ((Command)this.BtnSearchCommand).RaiseCanExecuteChanged();
            }
        }

        public string SearchString
        {
            get => this.mySearchString;

            set
            {
                if (string.Equals(this.mySearchString, value)) return;
                this.mySearchString = value;
                GridPartners = new ObservableCollection<Partner>(from emp in myAllPartners where emp.UserName.ToLower().Contains(value.ToLower()) select emp);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Partner> GridPartners
        {
            get => this.myGridPartners;
            set => SetProperty(ref myGridPartners, value);
        }

        public ObservableCollection<PartnerPayment> GridPartnerPayments
        {
            get => this.myGridPartnerPayments;
            set => SetProperty(ref myGridPartnerPayments, value);
        }

        public DateTime FromDate
        {
            get => this.myFromDate;
            set => SetProperty(ref myFromDate, value);
        }
        public DateTime ToDate
        {
            get => this.myToDate;
            set => SetProperty(ref myToDate, value);
        }

        public bool IsAllFilter
        {
            get => this.myIsAllFilter;
            set
            {
                SetProperty(ref myIsAllFilter, value);
                IsAutoCompleteTextBoxEnabled = !value;
                ((Command)this.BtnSearchCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsAutoCompleteTextBoxEnabled
        {
            get => this.myIsAutoCompleteTextBoxEnabled;
            set => SetProperty(ref myIsAutoCompleteTextBoxEnabled, value);
        }

        public async void Load()
        {
            Clear();
            myAllPartners = new List<Partner>(await myPartnerRepo.GetAllAsync());
            myAllPartnerPayments = new List<PartnerPayment>(await myPartnerPaymentRepo.GetAllAsync());
            foreach (var partnerPayment in myAllPartnerPayments)
            {
                partnerPayment.Employee = myAllPartners.FirstOrDefault(partner => partner.EmployeeId == partnerPayment.EmployeeId);
            }
        }

        private bool CanExecuteSearch(object arg)
        {
            return Partner != null && Partner.EmployeeId != 0 || IsAllFilter;
        }

        private void OnSearchButtonClick(object obj)
        {
            var filteredPartnerPayment = new List<PartnerPayment>();

            if (IsAllFilter)
            {
                filteredPartnerPayment = myAllPartnerPayments.Where(partnerPayment => partnerPayment.DateOfPayment > this.FromDate && partnerPayment.DateOfPayment < this.ToDate).ToList();
            }
            else
            {
                filteredPartnerPayment = myAllPartnerPayments.Where(partnerPayment => partnerPayment.DateOfPayment > this.FromDate && partnerPayment.DateOfPayment < this.ToDate && partnerPayment.EmployeeId == Partner.EmployeeId).ToList();
            }

            GridPartnerPayments = new ObservableCollection<PartnerPayment>(filteredPartnerPayment);
        }

        private void Clear()
        {
            Partner = new Partner();
            SearchString = string.Empty;
            GridPartnerPayments.Clear();
            FromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);
            IsAllFilter = false;
        }
    }
}
