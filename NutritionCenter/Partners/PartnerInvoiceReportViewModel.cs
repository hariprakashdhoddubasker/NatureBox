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

namespace NatureBox.Partners
{
    public class PartnerInvoiceReportViewModel : BaseViewModel
    {
        private Partner myEmployee;
        private string mySearchString;

        private List<Customer> myAllCustomers;
        private List<Product> myAllProducts;
        private List<Invoice> myAllInvoice;
        private ObservableCollection<Partner> myGridEmployees;
        private ObservableCollection<Invoice> myGridInvoices;
        private DateTime myFromDate = DateTime.Now;
        private DateTime myToDate = DateTime.Now;
        private double myTotalMRP;
        private double myTotalVolumnPoint;
        private List<Partner> myAllEmployees;
        private double myTotalExpense;
        private double myTotalCost;
        private bool myIsAutoCompleteTextBoxEnabled;
        private bool myIsAllFilter;
        private readonly IProductRepository myProductRepo;
        private readonly IInvoiceRepository myInvoiceRepo;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IPartnerRepository myEmployeeRepo;

        public PartnerInvoiceReportViewModel(ICustomerRepository customerRepo, IProductRepository productRepo, IInvoiceRepository invoiceRepo, IPartnerRepository employeeRepo)
        {
            this.myEmployeeRepo = employeeRepo;
            this.myCustomerRepo = customerRepo;
            this.myProductRepo = productRepo;
            this.myInvoiceRepo = invoiceRepo;
            this.myAllEmployees = new List<Partner>();
            this.myAllCustomers = new List<Customer>();
            this.BtnSearchCommand = new Command(this.OnSearchButtonClick, this.CanExecuteSearch);
            this.GridInvoices = new ObservableCollection<Invoice>();
            IsAllFilter = false;
            IsAutoCompleteTextBoxEnabled = true;
        }

        public ICommand BtnSearchCommand { get; private set; }

        public Partner Employee
        {
            get => this.myEmployee;
            set
            {
                SetProperty(ref myEmployee, value);
                ((Command)this.BtnSearchCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsAutoCompleteTextBoxEnabled
        {
            get => this.myIsAutoCompleteTextBoxEnabled;
            set => SetProperty(ref myIsAutoCompleteTextBoxEnabled, value);
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

        public ObservableCollection<Partner> GridEmployees
        {
            get => this.myGridEmployees;
            set => SetProperty(ref myGridEmployees, value);
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

        public double TotalVolumnPoint
        {
            get => this.myTotalVolumnPoint;
            set => SetProperty(ref myTotalVolumnPoint, value);
        }
        public double TotalExpense
        {
            get => this.myTotalExpense;
            set => SetProperty(ref myTotalExpense, value);
        }
        public double TotalCost
        {
            get => this.myTotalCost;
            set => SetProperty(ref myTotalCost, value);
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

        private bool CanExecuteSearch(object arg)
        {
            return (Employee != null && Employee.EmployeeId != 0) || IsAllFilter;
        }

        private void OnSearchButtonClick(object obj)
        {
            var employeeReferedCustomers = myAllCustomers.Where(customer => customer.EmployeeId == Employee.EmployeeId);
            var filteredInvoice = new List<Invoice>();

            if (IsAllFilter)
            {
                filteredInvoice = myAllInvoice.Where(invoice => invoice.DateOfPurchase > this.FromDate && invoice.DateOfPurchase < this.ToDate).ToList();
            }
            else
            {
                filteredInvoice = myAllInvoice.Where(invoice => invoice.DateOfPurchase > this.FromDate && invoice.DateOfPurchase < this.ToDate && employeeReferedCustomers.Contains(invoice.Customer)).ToList();
            }

            GridInvoices = new ObservableCollection<Invoice>(filteredInvoice);
            TotalMRP = filteredInvoice.Sum(invoice => invoice.Product.MRP * invoice.Quantity);
            TotalVolumnPoint = filteredInvoice.Sum(invoice => invoice.Product.VolumePoint);
            TotalExpense = filteredInvoice.Sum(invoice => invoice.Product.Expense);
            TotalCost = filteredInvoice.Sum(invoice => invoice.Product.Cost);
        }

        private void Clear()
        {
            Employee = new Partner();
            SearchString = string.Empty;
            GridInvoices.Clear();
            TotalVolumnPoint = 0;
            TotalMRP = 0;
            TotalExpense = 0;
            TotalCost = 0;
            FromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);
            IsAllFilter = false;
        }
    }
}
