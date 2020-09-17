using NatureBox.Commands;
using NatureBox.Customers.Service;
using NatureBox.Model;
using NatureBox.Service;
using NatureBox.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Ubiety.Dns.Core.Records;

namespace NatureBox.Customers
{

    public class HealthRecordViewModel : BaseViewModel
    {
        private string mySearchString;
        private ObservableCollection<Customer> myGridCustomers;
        private Customer myCustomer;
        private List<Customer> myAllCustomers;
        private List<HealthRecord> myAllHealthRecords;
        private ObservableCollection<HealthRecord> myGridHealthRecords;
        private HealthRecord mySelectedHealthRecord;
        private readonly ICustomerRepository myCustomerRepo;
        private readonly IHealthRecordRepository myHealthRecordRepo;

        public HealthRecordViewModel(ICustomerRepository customersRepo, IHealthRecordRepository healthRecordRepo)
        {
            this.myHealthRecordRepo = healthRecordRepo;
            this.myCustomerRepo = customersRepo;
            this.DeleteCommand = new Command(this.Delete, this.CanExecuteOnSelectedRecord);
            this.GridRowEditEndingCommand = new Command(this.OnGridRowEditEnding, this.CanExecuteOnSelectedRecord);
            this.BtnGridAddRowCommand = new Command(this.OnGridRowAdd, this.CanExecuteGridAddRow);
            this.BtnDownloadCommand = new Command(this.OnDownloadReport, this.CanExecuteDownloadReport);
            this.Customer = new Customer();
            this.myAllCustomers = new List<Customer>();
            this.GridHealthRecords = new ObservableCollection<HealthRecord>();
        }

        public ICommand DeleteCommand { get; private set; }
        public ICommand GridRowEditEndingCommand { get; private set; }
        public ICommand BtnGridAddRowCommand { get; private set; }
        public ICommand BtnDownloadCommand { get; private set; }
        
        public Customer Customer
        {
            get => this.myCustomer;
            set
            {
                if (object.Equals(myCustomer, value) || value == null) return;
                myCustomer = value;
                OnPropertyChanged();
                UpdateGrid();

                ((Command)this.BtnGridAddRowCommand).RaiseCanExecuteChanged();
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

        public ObservableCollection<Customer> GridCustomers
        {
            get => this.myGridCustomers;
            set => SetProperty(ref myGridCustomers, value);
        }

        public ObservableCollection<HealthRecord> GridHealthRecords
        {
            get => this.myGridHealthRecords;
            set => SetProperty(ref myGridHealthRecords, value);
        }

        public HealthRecord SelectedHealthRecord
        {
            get => this.mySelectedHealthRecord;
            set => SetProperty(ref mySelectedHealthRecord, value);
        }

        private bool CanExecuteOnSelectedRecord(object arg)
        {
            return true;
        }

        private bool CanExecuteGridAddRow(object arg)
        {
            return Customer != null && Customer.CustomerId != 0;
        }

        private async void Delete(object obj)
        {
            var healthRecord = (HealthRecord)obj;
            GridHealthRecords.Remove(healthRecord);
            UpdateGridFooterRows();
            if (healthRecord.HealthRecordId == 0)
            {
                return;
            }
            await myHealthRecordRepo.DeleteAsync(healthRecord.HealthRecordId);
            await GetAllHealthRecords();
        }

        public async void Load()
        {
            Clear();
            myAllCustomers = await myCustomerRepo.GetAllAsync();
            await GetAllHealthRecords();
        }

        private async Task GetAllHealthRecords()
        {
            myAllHealthRecords = await myHealthRecordRepo.GetAllAsync();
            myAllHealthRecords = myAllHealthRecords.OrderBy(record => record.RecordedDate).ToList();
        }

        private async void OnGridRowEditEnding(object obj)
        {
            if (!GridHealthRecords.Any())
            {
                return;
            }
            SelectedHealthRecord.CustomerId = Customer.CustomerId;

            if (SelectedHealthRecord.HealthRecordId != 0)
            {
                await myHealthRecordRepo.UpdateAsync(SelectedHealthRecord);
            }
            else
            {
                await myHealthRecordRepo.AddAsync(SelectedHealthRecord);
            }
            UpdateGridFooterRows();
            await GetAllHealthRecords();
        }

        private void OnGridRowAdd(object obj)
        {
            GridHealthRecords = new ObservableCollection<HealthRecord>(GetSelectedCustomerHealthRecords())
            {
                new HealthRecord()
            };
            UpdateGridFooterRows();
        }

        private bool CanExecuteDownloadReport(object arg)
        {
            return true;
        }

        private void OnDownloadReport(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
           var fileSavePath = HtmlReport.Generate(GridHealthRecords.ToList(), Customer);
            Mouse.OverrideCursor = null;
            UIService.ShowMessage($"Report downloaded {Environment.NewLine}{fileSavePath}");
        }


        private void UpdateGrid()
        {
            if (Customer.CustomerId != 0)
            {
                GridHealthRecords = new ObservableCollection<HealthRecord>(GetSelectedCustomerHealthRecords());
                UpdateGridFooterRows();
            }
        }

        private IEnumerable<HealthRecord> GetSelectedCustomerHealthRecords()
        {
            return myAllHealthRecords.Where(record => record.CustomerId == Customer.CustomerId).OrderBy(record => record.RecordedDate);
        }

        private void UpdateGridFooterRows()
        {
            GridHealthRecords = new ObservableCollection<HealthRecord>(GridHealthRecords.Where(record => record.HealthRecordId != -1).OrderBy(record => record.RecordedDate));

            if (!GridHealthRecords.Any() || GridHealthRecords.Count == 1)
            {
                return;
            }

            var recentChanges = GetDiffference(GridHealthRecords[^2], GridHealthRecords.Last());
            var totalChanges = GetDiffference(GridHealthRecords.First(), GridHealthRecords.Last());

            GridHealthRecords.Add(recentChanges);
            GridHealthRecords.Add(totalChanges);
        }

        private HealthRecord GetDiffference(HealthRecord healthRecord1, HealthRecord healthRecord2)
        {
            var difference = new HealthRecord
            {
                BMI = healthRecord1.BMI - healthRecord2.BMI,
                BMR = healthRecord1.BMR - healthRecord2.BMR,          
                Chest = healthRecord1.Chest - healthRecord2.Chest,
                Fat = healthRecord1.Fat - healthRecord2.Fat,
                Hip = healthRecord1.Hip - healthRecord2.Hip,           
                VFat = healthRecord1.VFat - healthRecord2.VFat,
                Waist = healthRecord1.Waist - healthRecord2.Waist,              
                Weight = healthRecord1.Weight - healthRecord2.Weight,

                BoneMass = healthRecord2.BoneMass - healthRecord1.BoneMass,
                MuscleMass = healthRecord2.MuscleMass - healthRecord1.MuscleMass,
                Water = healthRecord2.Water - healthRecord1.Water,

                HealthRecordId = -1,
                RecordedDateString = (healthRecord1.RecordedDate - healthRecord2.RecordedDate).Days.ToString(),
                ReadOnly = true
            };
            return difference;
        }

        private void Clear()
        {
            Customer = new Customer();
            SearchString = string.Empty;
            GridHealthRecords.Clear();
        }
    }
}
