using NatureBox.Commands;
using NatureBox.Customers.Service;
using NatureBox.Partners.Service;
using NatureBox.Model;
using NatureBox.Transactions.Service;
using NatureBox.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NatureBox.Customers
{

    public class CustomerPaymentReportViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> myGridCustomers;
        private Customer myCustomer;
        private string mySearchString;
        private List<Partner> myAllEmployees;
        private List<Customer> myAllCustomers;
        private List<CustomerPayment> myAllCustomerPayments;
        private ObservableCollection<CustomerPayment> myGridCustomerPayments;
        private DateTime myFromDate;
        private DateTime myToDate;
        private bool myIsAllFilter;
        private bool myIsAutoCompleteTextBoxEnabled;
        private readonly ICustomerPaymentRepository myCustomerPaymentRepo;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IPartnerRepository myEmployeeRepo;

        public CustomerPaymentReportViewModel(ICustomerPaymentRepository CustomerPaymentRepo, ICustomerRepository CustomerRepo, IPartnerRepository employeeRepo)
        {
            this.myCustomerRepo = CustomerRepo;
            this.myCustomerPaymentRepo = CustomerPaymentRepo;
            this.myEmployeeRepo = employeeRepo;
            this.myAllEmployees = new List<Partner>();
            this.myAllCustomers = new List<Customer>();
            this.BtnSearchCommand = new Command(this.OnSearchButtonClick, this.CanExecuteSearch);
            this.GridCustomerPayments = new ObservableCollection<CustomerPayment>();
            this.IsAllFilter = false;
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

        public ObservableCollection<CustomerPayment> GridCustomerPayments
        {
            get => this.myGridCustomerPayments;
            set => SetProperty(ref myGridCustomerPayments, value);
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
            myAllCustomers = new List<Customer>(await myCustomerRepo.GetAllAsync());
            myAllCustomerPayments = new List<CustomerPayment>(await myCustomerPaymentRepo.GetAllAsync());
            myAllEmployees = new List<Partner>(await myEmployeeRepo.GetAllAsync());

            foreach (var customer in myAllCustomers)
            {
                customer.Employee = myAllEmployees.FirstOrDefault(employee => employee.EmployeeId == customer.EmployeeId);
            }

            foreach (var customerPayment in myAllCustomerPayments)
            {
                customerPayment.Customer = myAllCustomers.FirstOrDefault(employee => employee.CustomerId == customerPayment.CustomerId);
            }
        }

        private bool CanExecuteSearch(object arg)
        {
            return Customer != null && Customer.EmployeeId != 0 || IsAllFilter;
        }

        private void OnSearchButtonClick(object obj)
        {
            var filteredCustomerPayment = new List<CustomerPayment>();

            if (IsAllFilter)
            {
                filteredCustomerPayment = myAllCustomerPayments.Where(CustomerPayment => CustomerPayment.DateOfPayment > this.FromDate && CustomerPayment.DateOfPayment < this.ToDate ).ToList();
            }
            else
            {
                filteredCustomerPayment = myAllCustomerPayments.Where(CustomerPayment => CustomerPayment.DateOfPayment > this.FromDate && CustomerPayment.DateOfPayment < this.ToDate && CustomerPayment.CustomerId == Customer.CustomerId).ToList();
            }

            GridCustomerPayments = new ObservableCollection<CustomerPayment>(filteredCustomerPayment);
        }

        private void Clear()
        {
            Customer = new Customer();
            SearchString = string.Empty;
            GridCustomerPayments.Clear();
            FromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);
            IsAllFilter = false;
        }
    }
}
