namespace NatureBox.Model
{
    using NatureBox.Common;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tb_invoice")]
    public class Invoice : AbstractNotifyPropertyChanged
    {
        private int myQuantity;
        private double myAmount;
        private bool myIsSettled;

        public Invoice()
        {
            DateOfPurchase = DateTime.Now;
            Quantity = 1;
        }
        [Key]
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public int Quantity
        {
            get => this.myQuantity;
            set => SetProperty(ref myQuantity, value);
        }
        public double Amount
        {
            get => this.myAmount;
            set => SetProperty(ref myAmount, value);
        }

        public bool IsSettled
        {
            get => this.myIsSettled;
            set => SetProperty(ref myIsSettled, value);
        }

        [NotMapped]
        public int SerialNumber { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
