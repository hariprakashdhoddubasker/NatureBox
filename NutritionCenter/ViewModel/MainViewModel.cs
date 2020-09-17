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

namespace NatureBox.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel mySelectedViewModel;
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

        public MainViewModel(IEventAggregator eventAggregator)
        {
            Task.Run(() =>
            {
                myEventAggregator = eventAggregator;
                myEventAggregator.GetEvent<NavigateViewsEvent>().Subscribe(UpdateViewModel);

                myEmployeeRegistrationViewModel = ContainerHelper.Container.Resolve<PartnerRegistrationViewModel>();
                myCustomerRegistrationViewModel = ContainerHelper.Container.Resolve<CustomerRegistrationViewModel>();
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

                this.SelectedViewModel = myInvoiceViewModel;
            });
        }

        public NavigationViewModel NavigationViewModel { get; set; }

        public BaseViewModel SelectedViewModel
        {
            get { return mySelectedViewModel; }
            set => SetProperty(ref mySelectedViewModel, value);
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

        public void RegisterNavigationViewModel()
        {
            NavigationViewModel = ContainerHelper.Container.Resolve<NavigationViewModel>();
        }
    }
}
