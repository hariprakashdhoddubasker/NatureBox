using NatureBox.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureBox.Model
{
    [Table("tb_partner_payment")]
    public class PartnerPayment : AbstractNotifyPropertyChanged
    {
        private int myPartnerPaymentId;
        private DateTime myDateOfPayment;
        private double myPaidAmount;
        private int myEmployeeId;
        private string myPaidType;
        private double myTotalVolumnPoint;
        private DateTime myBillingCycleFromDate;
        private DateTime myBillingCycleToDate;
        private int myTotalNoOfProducts;

        public PartnerPayment()
        {
            this.DateOfPayment = DateTime.Now;
        }

        [Key]
        public int PartnerPaymentId
        {
            get => this.myPartnerPaymentId;
            set => SetProperty(ref myPartnerPaymentId, value);
        }

        public int EmployeeId
        {
            get => this.myEmployeeId;
            set => SetProperty(ref myEmployeeId, value);
        }

        public DateTime DateOfPayment
        {
            get => this.myDateOfPayment;
            set => SetProperty(ref myDateOfPayment, value);
        }

        public DateTime BillingCycleFromDate
        {
            get => this.myBillingCycleFromDate;
            set => SetProperty(ref myBillingCycleFromDate, value);
        }

        public DateTime BillingCycleToDate
        {
            get => this.myBillingCycleToDate;
            set => SetProperty(ref myBillingCycleToDate, value);
        }

        public double PaidAmount
        {
            get => this.myPaidAmount;
            set => SetProperty(ref myPaidAmount, value);
        }

        public string PaidType
        {
            get => this.myPaidType;
            set => SetProperty(ref myPaidType, value);
        }

        public double TotalVolumnPoint
        {
            get => this.myTotalVolumnPoint;
            set => SetProperty(ref myTotalVolumnPoint, value);
        }

        public int TotalNoOfProducts
        {
            get => this.myTotalNoOfProducts;
            set => SetProperty(ref myTotalNoOfProducts, value);
        }

        public virtual Partner Employee { get; set; }
    }
}
