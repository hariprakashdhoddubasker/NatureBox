namespace NatureBox.Customers
{
    using NatureBox.Commands;
    using NatureBox.Customers.Service;
    using NatureBox.Partners.Service;
    using NatureBox.Model;
    using NatureBox.Service;
    using NatureBox.Transactions.Service;
    using NatureBox.ViewModel;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class CustomerPaymentViewModel : BaseViewModel
    {
        private List<Customer> myAllCustomers;
        private ObservableCollection<Customer> myGridCustomers;
        private Customer myCustomer;
        private CustomerPayment myPayment;
        private string mySearchString;
        private readonly ICustomerPaymentRepository myCustomerPaymentRepo;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IPartnerRepository myEmployeeRepo;

        public CustomerPaymentViewModel(ICustomerPaymentRepository customerPaymentRepo, ICustomerRepository customersRepo, IPartnerRepository employeeRepo)
        {
            myCustomerPaymentRepo = customerPaymentRepo;
            myCustomerRepo = customersRepo;
            myEmployeeRepo = employeeRepo;
            this.BtnPayCommand = new Command(this.BtnPayClick, o => true);
            this.CustomerPayment = new CustomerPayment();
            this.myAllCustomers = new List<Customer>();
            this.Customer = new Customer();
        }

        public ICommand BtnPayCommand { get; private set; }

        public Customer Customer
        {
            get => this.myCustomer;

            set
            {
                if (string.Equals(this.myCustomer, value) || value == null) return;
                this.myCustomer = value;
                UpdateCustomerDetails();
                OnPropertyChanged();
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

        public CustomerPayment CustomerPayment
        {
            get => this.myPayment;
            set => SetProperty(ref myPayment, value);
        }

        public ObservableCollection<Customer> GridCustomers
        {
            get => this.myGridCustomers;
            set => SetProperty(ref myGridCustomers, value);
        }

        private void UpdateCustomerDetails()
        {
            CustomerPayment.CustomerId = Customer.CustomerId;
        }

        private async void BtnPayClick(object obj)
        {
            bool? result = true;
            if (CustomerPayment.AmountPaid > 9999)
            {
                result = UIService.ShowMessage($"Amount Paid by {Customer.Name} is Rs.{CustomerPayment.AmountPaid}.Please confirm by clicking Ok", Visibility.Visible);
            }

            if (Customer.BalanceAmount + CustomerPayment.AmountPaid < 0)
            {
                UIService.ShowMessage($"Customer Amount cannot be in negative : {Customer.BalanceAmount + CustomerPayment.AmountPaid}");
                result = false;
            }

            if (result == true)
            {
                Customer.BalanceAmount += CustomerPayment.AmountPaid;
                Customer.TotalAmountPaid += CustomerPayment.AmountPaid;

                await Task.Run(() =>
                {
                    myCustomerRepo.UpdateAsync(Customer);
                }).ContinueWith((result) =>
                {
                    myCustomerPaymentRepo.AddAsync(CustomerPayment);
                });

                UIService.ShowMessage($"Payment of Rs.{CustomerPayment.AmountPaid} is added to {Customer.Name}'s account");
            }
            Load();
        }

        private void Clear()
        {
            this.CustomerPayment = new CustomerPayment();
            Customer = new Customer();
            SearchString = string.Empty;
        }

        public async void Load()
        {
            Clear();

            myAllCustomers = new List<Customer>(await myCustomerRepo.GetAllAsync());
            foreach (var customer in myAllCustomers)
            {
                customer.Employee = await myEmployeeRepo.GetEntityByIdAsync(customer.EmployeeId);
            }
        }
    }
}
