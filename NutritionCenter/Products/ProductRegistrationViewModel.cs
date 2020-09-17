namespace NatureBox.Products
{
    using NatureBox.Commands;
    using NatureBox.Model;
    using NatureBox.Products.Service;
    using NatureBox.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    public class ProductRegistrationViewModel : BaseViewModel
    {
        private ObservableCollection<Product> myGridProducts;
        private List<Product> myAllProducts;
        private string myButtonState;
        private Product myProduct;
        private readonly IProductRepository myProductRepo;

        public ProductRegistrationViewModel(IProductRepository productRepo)
        {
            myProductRepo = productRepo;
            this.BtnSaveUpdateCommand = new Command(this.BtnSaveUpdateClick, this.IsValidProduct);
            this.BtnDeleteCommand = new Command(this.BtnDeleteClick, this.IsValidProduct);
            this.GridUpdateCommand = new Command(this.GridUpdate, o => true);
            this.BtnSetAsDefaultCommand = new Command(this.OnSetAsDefaultClick, this.IsValidProduct);
            GridProducts = new ObservableCollection<Product>();
            myAllProducts = new List<Product>();
            this.Product = new Product();
            Product.PropertyChanged += Product_PropertyChanged;
        }

        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand BtnDeleteCommand { get; }
        public ICommand BtnSetAsDefaultCommand { get; }
        public ICommand GridUpdateCommand { get; }

        public Product Product
        {
            get => this.myProduct;

            set
            {
                if (string.Equals(this.myProduct, value)) return;
                this.myProduct = value;
                OnPropertyChanged();
            }
        }

        public string ButtonState
        {
            get => this.myButtonState;

            set
            {
                if (string.Equals(this.myButtonState, value)) return;
                this.myButtonState = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> GridProducts
        {
            get => this.myGridProducts;

            set
            {
                if (Equals(this.myGridProducts, value)) return;
                this.myGridProducts = value;
                OnPropertyChanged();
            }
        }

        private void OnSetAsDefaultClick(object obj)
        {
            foreach (var gridProduct in myAllProducts)
            {
                if (Product == gridProduct)
                {
                    if (gridProduct.IsDefaultProduct == 0)
                    {
                        gridProduct.IsDefaultProduct = 1;
                        myProductRepo.UpdateAsync(gridProduct);
                    }
                }
                else if (gridProduct.IsDefaultProduct == 1)
                {
                    gridProduct.IsDefaultProduct = 0;
                    myProductRepo.UpdateAsync(gridProduct);
                }
            }
            Load();
        }

        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ProductId")
            {
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool IsValidProduct(object arg)
        {
            return !string.IsNullOrEmpty(Product.Name) &&
                   double.TryParse(Product.MRP.ToString(), out _) &&
                   double.TryParse(Product.VolumePoint.ToString(), out _) &&
                   double.TryParse(Product.Expense.ToString(), out _) &&
                   !string.IsNullOrEmpty(Convert.ToString(Product.MRP)) &&
                   !string.IsNullOrEmpty(Convert.ToString(Product.VolumePoint)) &&
                   !string.IsNullOrEmpty(Convert.ToString(Product.Expense));

        }

        private void BtnSaveUpdateClick(object obj)
        {
            Product.Cost = Product.MRP - Product.Expense;

            if (ButtonState == "SAVE")
            {
                Product.IsDefaultProduct = 0;
                myProductRepo.AddAsync(Product);
            }
            else
            {
                myProductRepo.UpdateAsync(Product);
            }

            Load();
        }

        private void BtnDeleteClick(object obj)
        {
            myProductRepo.DeleteAsync(Product.ProductId);
            this.ButtonState = "SAVE";
            Load();
        }

        public async void Load()
        {
            Clear();
            var tes = await myProductRepo.GetAllAsync();
            myAllProducts = tes;
            GridProducts = new ObservableCollection<Product>(myAllProducts);
        }

        private void Clear()
        {
            Product.ProductId = 0;
            Product.Name = string.Empty;
            Product.MRP = 0;
            Product.VolumePoint = 0.0;
            Product.Expense = 0.0;
            ButtonState = "SAVE";
            RaiseCanExecuteChanged();
        }

        private void GridUpdate(object product)
        {
            this.Product = (Product)product;
            this.ButtonState = "UPDATE";
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnDeleteCommand).RaiseCanExecuteChanged();
            ((Command)this.BtnSetAsDefaultCommand).RaiseCanExecuteChanged();
            Product.PropertyChanged += Product_PropertyChanged;
        }
    }
}
