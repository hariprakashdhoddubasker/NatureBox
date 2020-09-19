namespace NatureBox.Model
{
    using NatureBox.Common;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tb_customer_payment")]
    public class CustomerPayment : AbstractNotifyPropertyChanged
    {
        private int myTransactionId;
        private int myCustomerId;
        private DateTime myDateOfPayment;
        private int myAmountPaid;

        public CustomerPayment()
        {
            DateOfPayment = DateTime.Now;
        }

        [Key]
        public int PaymentId
        {
            get => this.myTransactionId;
            set => SetProperty(ref myTransactionId, value);
        }

        public int CustomerId
        {
            get => this.myCustomerId;
            set => SetProperty(ref myCustomerId, value);
        }

        public DateTime DateOfPayment
        {
            get => this.myDateOfPayment;
            set => SetProperty(ref myDateOfPayment, value);
        }

        public int AmountPaid
        {
            get => this.myAmountPaid;
            set => SetProperty(ref myAmountPaid, value);
        }

        [NotMapped]
        public int SerialNumber { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
