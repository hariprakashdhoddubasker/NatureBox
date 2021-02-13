using NatureBox.Customers;
using NatureBox.Partners;
using NatureBox.Event;
using NatureBox.Products;
using NatureBox.Reports;
using NatureBox.Service;
using NatureBox.Transactions;
using Prism.Events;
using System.Threading.Tasks;
using Unity;
using System.Windows.Input;
using NatureBox.Commands;
using System;
using System.Collections.Generic;
using NatureBox.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace NatureBox.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel mySelectedViewModel;
        private ContactViewModel myContactViewModel;
        private PartnerRegistrationViewModel myEmployeeRegistrationViewModel;
        private CustomerRegistrationViewModel myCustomerRegistrationViewModel;
        private ProductRegistrationViewModel myProductRegistrationViewModel;
        private CustomerPaymentViewModel myCustomerPaymentViewModel;
        private InvoiceViewModel myInvoiceViewModel;
        private CustomerInvoiceReportViewModel myCustomerAttendanceViewModel;
        private PartnerInvoiceReportViewModel myEmployeeReportViewModel;
        private HealthRecordViewModel myHealthRecordViewModel;
        private BackUpRestoreViewModel myBackUpRestoreViewModel;
        private PartnerSettlementViewModel myPartnerPaymentViewModel;
        private PartnerSettlementReportViewModel myPartnerPaymentReportViewModel;
        private CustomerPaymentReportViewModel myCustomerPaymentReportViewModel;
        private IEventAggregator myEventAggregator;
        private string myCurrentUserName;
        private ObservableCollection<Customer> myBirthdayAlerts;

        public MainViewModel(IEventAggregator eventAggregator)
        {
            var task = Task.Run(() =>
             {
                 myEventAggregator = eventAggregator;
                 myEventAggregator.GetEvent<NavigateViewsEvent>().Subscribe(UpdateViewModel);
                 myEventAggregator.GetEvent<NotifyBirthDayAlerts>().Subscribe(NotifyBirthDayAlert);
                 myContactViewModel = ContainerHelper.Container.Resolve<ContactViewModel>();
                 myEmployeeRegistrationViewModel = ContainerHelper.Container.Resolve<PartnerRegistrationViewModel>();
                 myProductRegistrationViewModel = ContainerHelper.Container.Resolve<ProductRegistrationViewModel>();
                 myCustomerPaymentViewModel = ContainerHelper.Container.Resolve<CustomerPaymentViewModel>();
                 myPartnerPaymentViewModel = ContainerHelper.Container.Resolve<PartnerSettlementViewModel>();
                 myInvoiceViewModel = ContainerHelper.Container.Resolve<InvoiceViewModel>();
                 myCustomerAttendanceViewModel = ContainerHelper.Container.Resolve<CustomerInvoiceReportViewModel>();
                 myEmployeeReportViewModel = ContainerHelper.Container.Resolve<PartnerInvoiceReportViewModel>();
                 myPartnerPaymentReportViewModel = ContainerHelper.Container.Resolve<PartnerSettlementReportViewModel>();
                 myCustomerPaymentReportViewModel = ContainerHelper.Container.Resolve<CustomerPaymentReportViewModel>();
                 myHealthRecordViewModel = ContainerHelper.Container.Resolve<HealthRecordViewModel>();
                 myBackUpRestoreViewModel = ContainerHelper.Container.Resolve<BackUpRestoreViewModel>();
                 myCustomerRegistrationViewModel = ContainerHelper.Container.Resolve<CustomerRegistrationViewModel>();
                 BirthdayAlerts = new ObservableCollection<Customer>(myCustomerRegistrationViewModel.GetCustomerBirthdayAlerts());
                 this.SelectedViewModel = myInvoiceViewModel;
                 this.BtnContactCommand = new Command(this.OnContactClick, o => true);
             });
        }

        private void NotifyBirthDayAlert(List<Customer> customers)
        {
            BirthdayAlerts = new ObservableCollection<Customer>(customers);
        }

        public ICommand BtnContactCommand { get; private set; }

        public NavigationViewModel NavigationViewModel { get; set; }

        public BaseViewModel SelectedViewModel
        {
            get { return mySelectedViewModel; }
            set => SetProperty(ref mySelectedViewModel, value);
        }

        public string CurrentUserName
        {
            get { return myCurrentUserName; }
            set => SetProperty(ref myCurrentUserName, value);
        }


        public ObservableCollection<Customer> BirthdayAlerts
        {
            get { return myBirthdayAlerts; }
            set => SetProperty(ref myBirthdayAlerts, value);
        }


        private void UpdateViewModel(NatureBoxForms natureBoxForms)
        {
            switch (natureBoxForms)
            {
                case NatureBoxForms.Customer:
                    SelectedViewModel = myCustomerRegistrationViewModel;
                    break;
                case NatureBoxForms.Partner:
                    SelectedViewModel = myEmployeeRegistrationViewModel;
                    break;
                case NatureBoxForms.Product:
                    SelectedViewModel = myProductRegistrationViewModel;
                    break;
                case NatureBoxForms.HealthRecord:
                    SelectedViewModel = myHealthRecordViewModel;
                    break;
                case NatureBoxForms.CustomerPayment:
                    SelectedViewModel = myCustomerPaymentViewModel;
                    break;
                case NatureBoxForms.ParterSettlement:
                    SelectedViewModel = myPartnerPaymentViewModel;
                    break;
                case NatureBoxForms.Invoice:
                    SelectedViewModel = myInvoiceViewModel;
                    break;
                case NatureBoxForms.CustomerInvoiceReport:
                    SelectedViewModel = myCustomerAttendanceViewModel;
                    break;
                case NatureBoxForms.PartnerInvoiceReport:
                    SelectedViewModel = myEmployeeReportViewModel;
                    break;
                case NatureBoxForms.CustomerPaymentReport:
                    SelectedViewModel = myCustomerPaymentReportViewModel;
                    break;
                case NatureBoxForms.ParnterSettlementReport:
                    SelectedViewModel = myPartnerPaymentReportViewModel;
                    break;
                case NatureBoxForms.BackUp:
                    SelectedViewModel = myBackUpRestoreViewModel;
                    break;
                default:
                    break;
            }
        }

        private void OnContactClick(object obj)
        {
            SelectedViewModel = myContactViewModel;
        }

        public void RegisterNavigationViewModel()
        {
            NavigationViewModel = ContainerHelper.Container.Resolve<NavigationViewModel>();
        }

        public void Load()
        {
            this.CurrentUserName = "Hi, " + UIService.CurrentUser.UserName;
        }
    }
}
