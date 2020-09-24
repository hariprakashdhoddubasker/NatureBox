using NatureBox.Commands;
using NatureBox.Customers.Service;
using NatureBox.Partners.Service;
using NatureBox.Model;
using NatureBox.Products.Service;
using NatureBox.Transactions.Service;
using NatureBox.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NatureBox.Customers
{
    public class CustomerInvoiceReportViewModel : BaseViewModel
    {
        private Customer myCustomer;
        private string mySearchString;

        private List<Customer> myAllCustomers;
        private List<Partner> myAllEmployees;
        private List<Product> myAllProducts;
        private List<Invoice> myAllInvoice;
        private ObservableCollection<Customer> myGridCustomers;
        private ObservableCollection<Invoice> myGridInvoices;
        private DateTime myFromDate = DateTime.Now;
        private DateTime myToDate = DateTime.Now;
        private double myTotalMRP;
        private int myTotalAttendedDays;
        private bool myIsAllFilter;
        private bool myIsAutoCompleteTextBoxEnabled;
        private readonly IProductRepository myProductRepo;
        private readonly IInvoiceRepository myInvoiceRepo;
        private readonly IPartnerRepository myEmployeeRepo;
        private readonly ICustomerRepository myCustomerRepo;

        public CustomerInvoiceReportViewModel(ICustomerRepository customerRepo, IProductRepository productRepo, IInvoiceRepository invoiceRepo, IPartnerRepository employeeRepo)
        {
            this.myEmployeeRepo = employeeRepo;
            this.myCustomerRepo = customerRepo;
            this.myProductRepo = productRepo;
            this.myInvoiceRepo = invoiceRepo;
            this.myAllCustomers = new List<Customer>();
            this.myAllEmployees = new List<Partner>();
            this.BtnSearchCommand = new Command(this.OnSearchButtonClick, this.CanExecuteSearch);
            this.GridInvoices = new ObservableCollection<Invoice>();
            IsAllFilter = false;
            IsAutoCompleteTextBoxEnabled = true;
        }

        public ICommand BtnSearchCommand { get; private set; }

        public Customer Customer
        {
            get => this.myCustomer;
            set
            {
                SetProperty(ref myCustomer, value);
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
                GridCustomers = new ObservableCollection<Customer>(from emp in myAllCustomers where emp.Name.ToLower().Contains(value.ToLower()) select emp);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> GridCustomers
        {
            get => this.myGridCustomers;
            set => SetProperty(ref myGridCustomers, value);
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

        public double TotalMRP
        {
            get => this.myTotalMRP;
            set => SetProperty(ref myTotalMRP, value);
        }

        public int TotalAttendedDays
        {
            get => this.myTotalAttendedDays;
            set => SetProperty(ref myTotalAttendedDays, value);
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
            myAllEmployees = new List<Partner>(await myEmployeeRepo.GetAllAsync());
            myAllCustomers = new List<Customer>(await myCustomerRepo.GetAllAsync());
            myAllProducts = new List<Product>(await myProductRepo.GetAllAsync());
            myAllInvoice = new List<Invoice>(await myInvoiceRepo.GetAllAsync());

            foreach (var customer in myAllCustomers)
            {
                customer.Employee = myAllEmployees.FirstOrDefault(employee => employee.EmployeeId == customer.EmployeeId);
            }

            foreach (var invoice in myAllInvoice)
            {
                invoice.Customer = myAllCustomers.FirstOrDefault(customer => customer.CustomerId == invoice.CustomerId);
                invoice.Product = myAllProducts.FirstOrDefault(product => product.ProductId == invoice.ProductId);
            }
        }

        private void UpdateGrid()
        {
            var filteredInvoice = new List<Invoice>();

            if (this.ToDate.Hour == 0)
            {
                this.ToDate += new TimeSpan(23, 59, 0);
            }

            if (IsAllFilter)
            {
                filteredInvoice = myAllInvoice.Where(invoice => invoice.DateOfPurchase > this.FromDate && invoice.DateOfPurchase < this.ToDate).ToList();
            }
            else
            {
                filteredInvoice = myAllInvoice.Where(invoice => invoice.DateOfPurchase > this.FromDate && invoice.DateOfPurchase < this.ToDate && invoice.Customer == Customer).ToList();
            }

            GridInvoices = new ObservableCollection<Invoice>(filteredInvoice);
            TotalMRP = filteredInvoice.Sum(invoice => invoice.Product.MRP * invoice.Quantity);
            TotalAttendedDays = filteredInvoice.Count();
        }

        private bool CanExecuteSearch(object arg)
        {
            return (Customer != null && Customer.CustomerId != 0) || IsAllFilter;
        }

        private void OnSearchButtonClick(object obj)
        {
            UpdateGrid();
        }

        private void Clear()
        {
            Customer = new Customer();
            SearchString = string.Empty;
            GridInvoices.Clear();
            TotalAttendedDays = 0;
            TotalMRP = 0;
            FromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);
            IsAllFilter = false;
        }
    }
}
