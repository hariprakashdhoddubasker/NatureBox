namespace NatureBox.Partners
{
    using NatureBox.Commands;
    using NatureBox.Partners.Service;
    using NatureBox.Model;
    using NatureBox.Service;
    using NatureBox.ViewModel;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public class PartnerRegistrationViewModel : BaseViewModel
    {
        private IPartnerRepository myRepo;
        public ICommand BtnSaveUpdateCommand { get; }
        public ICommand GridUpdateCommand { get; }

        private ObservableCollection<Partner> myGridEmployees;
        private string myButtonState;
        private Partner myEmployee;
        private List<string> myRoles;

        public PartnerRegistrationViewModel(IPartnerRepository repo)
        {
            myRepo = repo;
            this.BtnSaveUpdateCommand = new Command(this.BtnSaveUpdateClick, this.CanSaveUpdate);
            this.GridUpdateCommand = new Command(this.GridUpdate, o => true);
            GridEmployees = new ObservableCollection<Partner>();
            this.Employee = new Partner();
        }

        private void Employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "EmployeeId")
            {
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        public Partner Employee
        {
            get => this.myEmployee;
            set => SetProperty(ref myEmployee, value);
        }

        public string ButtonState
        {
            get => this.myButtonState;
            set => SetProperty(ref myButtonState, value);
        }

        public ObservableCollection<Partner> GridEmployees
        {
            get => this.myGridEmployees;
            set => SetProperty(ref myGridEmployees, value);
        }

        public List<string> Roles
        {
            get => this.myRoles;
            set
            {
                SetProperty(ref myRoles, value);
                ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool CanSaveUpdate(object arg)
        {
            return this.IsValidEmployeeEnties();
        }

        private void BtnSaveUpdateClick(object obj)
        {
            if (ButtonState== "SAVE")
            {
                Employee.EmployeeId = GridEmployees.Count + 1;
                myRepo.AddAsync(Employee);
            }
            else
            {
                myRepo.UpdateAsync(Employee);
            }
       
            Clear();
            GridFill();
        }

        private void GridFill()
        {
            GridEmployees.Clear();
            GridEmployees = new ObservableCollection<Partner>(myRepo.GetAllAsync().Result);
        }

        public async void Load()
        {
            Clear();
            GridEmployees = new ObservableCollection<Partner>(await myRepo.GetAllAsync());
            Roles = new List<string> { NatureBoxRoles.Admin.ToString(), NatureBoxRoles.Parnter.ToString() };
            Employee.PropertyChanged += Employee_PropertyChanged;
        }

        public bool IsValidEmployeeEnties()
        {
            return !string.IsNullOrEmpty(Employee.UserName) &&
                    !string.IsNullOrEmpty(Employee.Password) &&
                    long.TryParse(Employee.MobileNumber.ToString(), out _) &&
                    Employee.MobileNumber.ToString().Length == 10 &&
                    !string.IsNullOrEmpty(Employee.Role);
        }

        public void Clear()
        {
            Employee = new Partner();
            this.ButtonState = "SAVE";
        }

        private void GridUpdate(object employee)
        {
            this.Employee = (Partner)employee;
            this.ButtonState = "UPDATE";
            ((Command)this.BtnSaveUpdateCommand).RaiseCanExecuteChanged();
        }
    }
}
