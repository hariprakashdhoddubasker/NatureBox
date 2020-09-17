using NatureBox.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureBox.Model
{
    [Table("tb_employee")]
    public class Partner : AbstractNotifyPropertyChanged
    {
        private string myUserName;
        private string myPassword;
        private long myMobileNumber;
        private string myRole;

        [Key]
        public int EmployeeId { get; set; }

        public string UserName
        {
            get => this.myUserName;
            set => SetProperty(ref myUserName, value);
        }

        public string Password
        {
            get => this.myPassword;
            set => SetProperty(ref myPassword, value);
        }

        public long MobileNumber
        {
            get => this.myMobileNumber;
            set => SetProperty(ref myMobileNumber, value);
        }

        public string Role
        {
            get => this.myRole;
            set => SetProperty(ref myRole, value);
        }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<PartnerPayment> PartnerPayments { get; set; }
        public override string ToString()
        {
            return UserName;
        }
    }
}
