namespace NatureBox.Transactions
{
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

    public class InvoiceViewModel : BaseViewModel
    {
        private string mySearchString;
        private ObservableCollection<Customer> myGridCustomers;
        private Customer myCustomer;
        private Product mySelectedProduct;
        private ObservableCollection<Product> myProducts;
        private ObservableCollection<Invoice> myGridInvoices;
        private int myProductNamesSelectedIndex;
        private Invoice myInvoice;
        private List<Customer> myAllCustomers;
        private readonly IInvoiceRepository myInvoiceRepo;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IProductRepository myProductRepo;
        private readonly IPartnerRepository myEmployeeRepo;

        public InvoiceViewModel(IInvoiceRepository invoiceRepo, ICustomerRepository customersRepo, IProductRepository productRepo, IPartnerRepository employeeRepo)
        {
            myInvoiceRepo = invoiceRepo;
            myCustomerRepo = customersRepo;
            myProductRepo = productRepo;
            myEmployeeRepo = employeeRepo;
            this.BtnSendSMSCommand = new Command(this.OnSendSMSClick, this.Validate);
            this.GridRowDeleteCommand = new Command(this.OnGridRowDeleteClick, o => true);
            this.Invoice = new Invoice();
            this.SelectedProduct = new Product();
            this.Customer = new Customer();
            Invoice.PropertyChanged += Invoice_PropertyChanged;
            myAllCustomers = new List<Customer>();
        }

        public ICommand BtnSendSMSCommand { get; private set; }
        public ICommand GridRowDeleteCommand { get; private set; }

        public Customer Customer
        {
            get => this.myCustomer;

            set
            {
                SetProperty(ref myCustomer, value);
                if (value == null)
                {
                    return;
                }
                CalculateInvoiceAmount();
                Invoice.CustomerId = Customer.CustomerId;
            }
        }

        public Product SelectedProduct
        {
            get => this.mySelectedProduct;

            set
            {
                SetProperty(ref mySelectedProduct, value);
                if (value == null)
                {
                    return;
                }
                CalculateInvoiceAmount();
                Invoice.ProductId = SelectedProduct.ProductId;
                ((Command)this.BtnSendSMSCommand).RaiseCanExecuteChanged();
            }
        }

        public Invoice Invoice
        {
            get => this.myInvoice;
            set => SetProperty(ref myInvoice, value);
        }

        public ObservableCollection<Invoice> GridInvoices
        {
            get => this.myGridInvoices;
            set => SetProperty(ref myGridInvoices, value);
        }

        public ObservableCollection<Product> Products
        {
            get => this.myProducts;
            set => SetProperty(ref myProducts, value);
        }

        public int ProductNamesSelectedIndex
        {
            get => myProductNamesSelectedIndex;
            set => SetProperty(ref myProductNamesSelectedIndex, value);
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

        private async void OnSendSMSClick(object obj)
        {
            Customer.BalanceAmount -= Invoice.Amount;
            string smsStatus = string.Empty;

            await myCustomerRepo.UpdateAsync(Customer).ContinueWith((result) =>
             {
                 var savedInvoice = myInvoiceRepo.AddAsync(Invoice);

                 if (savedInvoice != null)
                 {
                     //string smsStatus = string.Empty;
                     smsStatus = new SmsService().SendInvoiceMessage(Customer, Invoice);

                     UIService.ShowMessage(smsStatus);
                 }
             });

            Load();
        }

        private bool Validate(object arg)
        {
            if (Customer == null || Invoice == null || Invoice.ProductId == 0)
            {
                return false;
            }
            return Customer.BalanceAmount >= Invoice.Amount && Invoice.Quantity > 0;
        }

        private void Invoice_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                CalculateInvoiceAmount();
            }
        }

        private void CalculateInvoiceAmount()
        {
            if (SelectedProduct == null || Invoice == null)
            {
                return;
            }
            Invoice.Amount = Invoice.Quantity * SelectedProduct.MRP;
            ((Command)this.BtnSendSMSCommand).RaiseCanExecuteChanged();
        }

        public async void Load()
        {
            Clear();
            myAllCustomers = new List<Customer>(await myCustomerRepo.GetAllAsync());
            var allEmployees = await myEmployeeRepo.GetAllAsync();

            foreach (var customer in myAllCustomers)
            {
                customer.Employee = allEmployees.FirstOrDefault(employee => employee.EmployeeId == customer.EmployeeId);
            }
            var test = await myProductRepo.GetAllAsync();
            var allProducts = await myProductRepo.GetAllAsync();
            var defaultProduct = allProducts.FirstOrDefault(product => product.IsDefaultProduct == 1);

            Products = new ObservableCollection<Product>(allProducts);

            if (defaultProduct != null)
            {
                SelectedProduct = defaultProduct;
            }

            var tempInvoices = await myInvoiceRepo.GetAllAsync();
            DateTime fromDate = DateTime.Now.Date + new TimeSpan(00, 01, 0);
            DateTime toDate = DateTime.Now.Date + new TimeSpan(23, 59, 0);
            tempInvoices = tempInvoices.Where(invoice => invoice.DateOfPurchase > fromDate && invoice.DateOfPurchase < toDate).ToList();
            var index = 1;
            foreach (var invoice in tempInvoices)
            {
                invoice.Customer = myAllCustomers.FirstOrDefault(customer => customer.CustomerId == invoice.CustomerId);
                invoice.Product = allProducts.FirstOrDefault(product => product.ProductId == invoice.ProductId);
                invoice.SerialNumber = index++;
            }

            GridInvoices = new ObservableCollection<Invoice>(tempInvoices);
        }

        private async void OnGridRowDeleteClick(object obj)
        {
            var currentGridRowInvoice = (Invoice)obj;
            currentGridRowInvoice.Customer.BalanceAmount += currentGridRowInvoice.Amount;
            var isInvoiceDeleted = false;

            await myCustomerRepo.UpdateAsync(currentGridRowInvoice.Customer).ContinueWith((result) =>
            {
                var isInvoiceDeleted = myInvoiceRepo.DeleteAsync(((Invoice)obj).InvoiceId).Result;
            });

            GridInvoices.Remove(currentGridRowInvoice);

            if (isInvoiceDeleted)
            {
                UIService.ShowMessage($"{Customer.Name}' invoice deleted, current Balance : {Customer.BalanceAmount}");
            }
            Clear();
        }

        private void Clear()
        {
            Customer = new Customer();
            SearchString = string.Empty;
            Invoice.Quantity = 1;
            Invoice.Amount = 0;
            Invoice.InvoiceId = 0;
        }
    }
}
