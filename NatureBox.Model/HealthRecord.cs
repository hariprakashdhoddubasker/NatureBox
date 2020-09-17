using NatureBox.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatureBox.Model
{
    [Table("tb_health_record")]
    public class HealthRecord : AbstractNotifyPropertyChanged
    {
        private int myHealthRecordId;
        private DateTime myRecordedDate;
        private double myChest;
        private double myWeight;
        private double myWaist;
        private double myHip;
        private double myBMI;
        private double myFat;
        private double myBoneMass;
        private double myMuscleMass;
        private double myWater;
        private double myVFat;
        private double myBMR;
        private int myCustomerId;
        private bool myReadOnly;
        private string myRecordedDateString;

        public HealthRecord()
        {
            RecordedDate = DateTime.Now;
            ReadOnly = false;
        }

        [Key]
        public int HealthRecordId
        {
            get => this.myHealthRecordId;
            set => SetProperty(ref myHealthRecordId, value);
        }
        public DateTime RecordedDate
        {
            get => this.myRecordedDate;
            set
            {
                SetProperty(ref myRecordedDate, value);
                RecordedDateString = RecordedDate.ToString("ddMMMyyyy");
            }
        }
        public double Weight
        {
            get => this.myWeight;
            set => SetProperty(ref myWeight, value);
        }

        public double Chest
        {
            get => this.myChest;
            set => SetProperty(ref myChest, value);
        }

        public double Waist
        {
            get => this.myWaist;
            set => SetProperty(ref myWaist, value);
        }
        public double Hip
        {
            get => this.myHip;
            set => SetProperty(ref myHip, value);
        }
        public double BMI
        {
            get => this.myBMI;
            set => SetProperty(ref myBMI, value);
        }
        public double BMR
        {
            get => this.myBMR;
            set => SetProperty(ref myBMR, value);
        }
        public double Fat
        {
            get => this.myFat;
            set => SetProperty(ref myFat, value);
        }
        public double VFat
        {
            get => this.myVFat;
            set => SetProperty(ref myVFat, value);
        }
        public double BoneMass
        {
            get => this.myBoneMass;
            set => SetProperty(ref myBoneMass, value);
        }
        public double MuscleMass
        {
            get => this.myMuscleMass;
            set => SetProperty(ref myMuscleMass, value);
        }
        public double Water
        {
            get => this.myWater;
            set => SetProperty(ref myWater, value);
        }

        public int CustomerId
        {
            get => this.myCustomerId;
            set => SetProperty(ref myCustomerId, value);
        }
        [NotMapped]
        public string RecordedDateString
        {
            get => this.myRecordedDateString;
            set => SetProperty(ref myRecordedDateString, value);
        }

        [NotMapped]
        public bool ReadOnly
        {
            get => this.myReadOnly;
            set => SetProperty(ref myReadOnly, value);
        }
        public virtual Customer Customer { get; set; }
    }
}
