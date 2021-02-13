namespace NatureBox.Customers
{
    using NatureBox.Commands;
    using NatureBox.Customers.Service;
    using NatureBox.Partners.Service;
    using NatureBox.Model;
    using NatureBox.ViewModel;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using NatureBox.Event;
    using Prism.Events;

    public class CustomerRegistrationViewModel : BaseViewModel
    {
        private ICustomerRepository myCustomerRepo;
        private IPartnerRepository myEmployeeRepo;
        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand BtnDeleteCommand { get; }
        public ICommand GridUpdateCommand { get; }
        private readonly IEventAggregator myEventAggregator;
        private ObservableCollection<Customer> myGridCustomers;
        private string myButtonState;
        private Customer myCustomer;
        private ObservableCollection<Partner> myReferrableEmployees;

        public CustomerRegistrationViewModel(ICustomerRepository customerRepo, IPartnerRepository employeeRepo, IEventAggregator eventAggregator)
        {
            myEventAggregator = eventAggregator;
            myCustomerRepo = customerRepo;
            myEmployeeRepo = employeeRepo;
            this.Customer = new Customer();
            this.BtnSaveUpdateCommand = new Command(this.BtnSaveUpdateClick, this.IsValidCustomer);
            SelectedReferredEmployee = new Partner();
            this.BtnDeleteCommand = new Command(this.BtnDeleteClick, this.IsValidCustomer);
            this.GridUpdateCommand = new Command(this.GridUpdate, o => true);
            GridCustomers = new ObservableCollection<Customer>();
            Customer.PropertyChanged += Customer_PropertyChanged;
        }

        public Customer Customer
        {
            get => this.myCustomer;
            set => SetProperty(ref myCustomer, value);
        }

        public string ButtonState
        {
            get => this.myButtonState;
            set => SetProperty(ref myButtonState, value);
        }

        public ObservableCollection<Customer> GridCustomers
        {
            get => this.myGridCustomers;
            set => SetProperty(ref myGridCustomers, value);
        }

        public ObservableCollection<Partner> ReferrableEmployees
        {
            get => this.myReferrableEmployees;
            set => SetProperty(ref myReferrableEmployees, value);
        }
        public Partner SelectedReferredEmployee
        {
            get => this.mySelectedReferredEmployee;
            set
            {
                SetProperty(ref mySelectedReferredEmployee, value);
                if (value == null)
                {
                    return;
                }
                Customer.EmployeeId = mySelectedReferredEmployee.EmployeeId;
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private int myReferredBySelectedIndex;
        private Partner mySelectedReferredEmployee;

        public int ReferredBySelectedIndex
        {
            get => myReferredBySelectedIndex;
            set => SetProperty(ref myReferredBySelectedIndex, value);
        }


        public async void Load()
        {
            Clear();
            ReferrableEmployees = new ObservableCollection<Partner>(await myEmployeeRepo.GetAllAsync());
            GridCustomers = await GetAllCustomers();

            foreach (var customer in GridCustomers)
            {
                customer.Employee = ReferrableEmployees.FirstOrDefault(employee => employee.EmployeeId == customer.EmployeeId);
            }
        }

        private async Task<ObservableCollection<Customer>> GetAllCustomers()
        {
            return new ObservableCollection<Customer>(await myCustomerRepo.GetAllAsync());
        }
        public IEnumerable<Customer> GetCustomerBirthdayAlerts()
        {
            var allCustomers = GetAllCustomers().Result;
            return allCustomers.Where(customer => customer.DOB.Date == DateTime.Now.Date || customer.DOB.Date == DateTime.Now.Date.AddDays(1));
        }

        private void Customer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CustomerId")
            {
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool IsValidCustomer(object arg)
        {
            return !string.IsNullOrEmpty(Customer.Name) &&
                Customer.DOB != null &&
                Customer.DOJ != null &&
                long.TryParse(Customer.MobileNumber.ToString(), out _) &&
                Customer.MobileNumber.ToString().Length == 10 &&
                int.TryParse(Customer.EmployeeId.ToString(), out _) &&
                mySelectedReferredEmployee != null;
        }

        private void BtnSaveUpdateClick(object obj)
        {
            if (Customer.EmployeeId == 0)
            {
                Customer.EmployeeId = mySelectedReferredEmployee.EmployeeId;
            }
            Customer.Employee = ReferrableEmployees.FirstOrDefault(employee => employee.EmployeeId == Customer.EmployeeId);

            if (ButtonState == "SAVE")
            {
                myCustomerRepo.AddAsync(Customer);
            }
            else
            {
                myCustomerRepo.UpdateAsync(Customer);
            }            
            Load();
            myEventAggregator.GetEvent<NotifyBirthDayAlerts>().Publish(GetCustomerBirthdayAlerts().ToList());
        }

        private void BtnDeleteClick(object obj)
        {
            myCustomerRepo.DeleteAsync(Customer.CustomerId);
            this.ButtonState = "SAVE";
            Load();
        }

        private void Clear()
        {
            Customer.CustomerId = 0;
            Customer.Name = string.Empty;
            Customer.DOB = DateTime.Now;
            Customer.DOJ = DateTime.Now;
            Customer.EmployeeId = 0;
            Customer.MobileNumber = 0;
            Customer.BalanceAmount = 0;
            Customer.TotalAmountPaid = 0;

            this.ButtonState = "SAVE";
            this.ReferredBySelectedIndex = 0;
            RaiseCanExecuteChanged();
        }

        private void GridUpdate(object customer)
        {
            this.Customer = (Customer)customer;
            SelectedReferredEmployee = Customer.Employee;
            this.ButtonState = "UPDATE";
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnDeleteCommand).RaiseCanExecuteChanged();
            Customer.PropertyChanged += Customer_PropertyChanged;
        }
    }
}
