using NatureBox.Commands;
using NatureBox.Customers.Service;
using NatureBox.Partners.Service;
using NatureBox.Model;
using NatureBox.Products.Service;
using NatureBox.Service;
using NatureBox.Transactions.Service;
using NatureBox.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace NatureBox.Partners
{
    public class PartnerSettlementViewModel : BaseViewModel
    {
        private string mySearchString;
        private Partner myEmployee;
        private ObservableCollection<Partner> myGridEmployees;
        private List<Partner> myAllEmployees;
        private List<Invoice> myAllInvoice;
        private List<Customer> myAllCustomers;
        private List<Product> myAllProducts;
        private readonly IPartnerPaymentRepository myPartnerPaymentRepo;
        private readonly IPartnerRepository myEmployeeRepo;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IInvoiceRepository myInvoiceRepo;
        private readonly IProductRepository myProductRepo;
        private DateTime myFromDate = DateTime.Now;
        private DateTime myToDate = DateTime.Now;
        private ObservableCollection<Invoice> myGridInvoices;
        private double myTotalCost;
        private List<string> myPaymentTypes;
        private string mySelectedPaymentType;
        private PartnerPayment myPartnerPayment;

        public PartnerSettlementViewModel(IPartnerPaymentRepository partnerPaymentRepo, IInvoiceRepository invoiceRepo, IProductRepository productRepo, IPartnerRepository employeeRepo, ICustomerRepository customerRepo)
        {
            this.myPartnerPaymentRepo = partnerPaymentRepo;
            this.myEmployeeRepo = employeeRepo;
            this.myInvoiceRepo = invoiceRepo;
            this.myProductRepo = productRepo;
            this.myCustomerRepo = customerRepo;
            this.myAllEmployees = new List<Partner>();
            this.myAllCustomers = new List<Customer>();
            this.PartnerPayment = new PartnerPayment();
            this.PartnerPayment.PropertyChanged += PartnerPayment_PropertyChanged;
            this.GridEmployees = new ObservableCollection<Partner>();
            this.BtnSearchCommand = new Command(this.OnSearchButtonClick, this.CanExecuteSearch);
            this.BtnPayCommand = new Command(this.OnPayClick, this.CanExecutePay);
            this.GridInvoices = new ObservableCollection<Invoice>();            
        }

        public ObservableCollection<Partner> GridEmployees
        {
            get => this.myGridEmployees;
            set => SetProperty(ref myGridEmployees, value);
        }

        public string SearchString
        {
            get => this.mySearchString;

            set
            {
                if (string.Equals(this.mySearchString, value)) return;
                this.mySearchString = value;
                GridEmployees = new ObservableCollection<Partner>(from emp in myAllEmployees where emp.UserName.ToLower().Contains(value.ToLower()) select emp);
                OnPropertyChanged();
            }
        }

        public Partner Employee
        {
            get => this.myEmployee;

            set
            {
                SetProperty(ref myEmployee, value);
                ((Command)this.BtnSearchCommand).RaiseCanExecuteChanged();
            }
        }

        public PartnerPayment PartnerPayment
        {
            get => this.myPartnerPayment;

            set
            {
                SetProperty(ref myPartnerPayment, value);
            }
        }

        public ObservableCollection<Invoice> GridInvoices
        {
            get => this.myGridInvoices;
            set => SetProperty(ref myGridInvoices, value);
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

        public double TotalCost
        {
            get => this.myTotalCost;
            set => SetProperty(ref myTotalCost, value);
        }

        public List<string> PaymentTypes
        {
            get => this.myPaymentTypes;
            set
            {
                SetProperty(ref myPaymentTypes, value);
                ((Command)this.BtnPayCommand).RaiseCanExecuteChanged();
            }
        }

        public string SelectedPaymentType
        {
            get => this.mySelectedPaymentType;
            set
            {
                SetProperty(ref mySelectedPaymentType, value);

                if (value == "NetWorth")
                {
                    PartnerPayment.PaidAmount = TotalCost;
                }
                else
                {
                    PartnerPayment.PaidAmount = 0.0;
                }
            }
        }

        public ICommand BtnSearchCommand { get; private set; }
        public ICommand BtnPayCommand { get; private set; }

        public async void Load()
        {
            Clear();
            myAllEmployees = new List<Partner>(await myEmployeeRepo.GetAllAsync());
            myAllCustomers = new List<Customer>(await myCustomerRepo.GetAllAsync());
            myAllInvoice = new List<Invoice>(await myInvoiceRepo.GetAllAsync());
            myAllProducts = new List<Product>(await myProductRepo.GetAllAsync());

            foreach (var invoice in myAllInvoice)
            {
                invoice.Customer = myAllCustomers.FirstOrDefault(customer => customer.CustomerId == invoice.CustomerId);
                invoice.Product = myAllProducts.FirstOrDefault(product => product.ProductId == invoice.ProductId);
            }
            PaymentTypes = new List<string> { "NetWorth", "Profit" };
            this.PartnerPayment.PropertyChanged += PartnerPayment_PropertyChanged;
        }

        private void Clear()
        {
            this.Employee = new Partner();
            this.SearchString = string.Empty;
            this.PartnerPayment = new PartnerPayment();
            this.TotalCost = 0;
            this.GridInvoices.Clear();
            this.FromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            this.ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);            
        }

        private bool CanExecuteSearch(object arg)
        {
            return Employee != null && Employee.EmployeeId != 0;
        }

        private void OnSearchButtonClick(object obj)
        {
            var employeeReferedCustomers = myAllCustomers.Where(customer => customer.EmployeeId == Employee.EmployeeId);

            var filteredInvoice = myAllInvoice.Where(invoice => !invoice.IsSettled && invoice.DateOfPurchase >= this.FromDate && invoice.DateOfPurchase <= this.ToDate && employeeReferedCustomers.Contains(invoice.Customer)).ToList();

            if (!filteredInvoice.Any())
            {
                UIService.ShowMessage($"There is no Invoice for given filter or all invoice are settled to {Employee.UserName}");
                return;
            }

            GridInvoices = new ObservableCollection<Invoice>(filteredInvoice);

            PartnerPayment.TotalNoOfProducts = filteredInvoice.Sum(invoice => invoice.Quantity);
            PartnerPayment.TotalVolumnPoint = filteredInvoice.Sum(invoice => invoice.Quantity * invoice.Product.VolumePoint);
            TotalCost = filteredInvoice.Sum(invoice => invoice.Quantity * invoice.Product.Cost);
        }

        private bool CanExecutePay(object arg)
        {
            return PartnerPayment.PaidAmount > 0 &&
                    !string.IsNullOrEmpty(PartnerPayment.PaidType) &&
                    PartnerPayment.TotalNoOfProducts > 0 &&
                    PartnerPayment.TotalVolumnPoint > 0;
        }

        private void OnPayClick(object obj)
        {
            GridInvoices.ToList().ForEach(c => c.IsSettled = true);
            myInvoiceRepo.UpdateRangeAsync(GridInvoices.ToList());
            PartnerPayment.BillingCycleFromDate = FromDate;
            PartnerPayment.BillingCycleToDate = ToDate;
            PartnerPayment.EmployeeId = Employee.EmployeeId;
            myPartnerPaymentRepo.AddAsync(PartnerPayment);
            this.Load();
        }

        private void PartnerPayment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CustomerId")
            {
                ((Command)this.BtnPayCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
