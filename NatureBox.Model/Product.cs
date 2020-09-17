namespace NatureBox.Model
{
    using NatureBox.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tb_product")]
    public class Product : AbstractNotifyPropertyChanged
    {
        private int myProductId;
        private string myName;
        private double myMRP;
        private double myVolumePoint;
        private double myExpense;
        private double myCost;
        private int myIsDefaultProduct;

        public int ProductId
        {
            get => this.myProductId;
            set => SetProperty(ref myProductId, value);
        }

        public string Name
        {
            get => this.myName;
            set => SetProperty(ref myName, value);
        }

        public double MRP
        {
            get => this.myMRP;
            set => SetProperty(ref myMRP, value);
        }

        public double VolumePoint
        {
            get => this.myVolumePoint;
            set => SetProperty(ref myVolumePoint, value);
        }

        public double Expense
        {
            get => this.myExpense;
            set => SetProperty(ref myExpense, value);
        }

        public double Cost
        {
            get => this.myCost;
            set => SetProperty(ref myCost, value);
        }

        public int IsDefaultProduct
        {
            get => this.myIsDefaultProduct;
            set => SetProperty(ref myIsDefaultProduct, value);
        }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
