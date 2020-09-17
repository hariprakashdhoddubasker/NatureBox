namespace NatureBox.Partners
{
    using NatureBox.Commands;
    using NatureBox.Common;
    using NatureBox.Partners.Service;
    using NatureBox.Model;
    using NatureBox.Service;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class LoginWindowViewModel : AbstractNotifyPropertyChanged
    {
        private IPartnerRepository _repo;
        private List<Partner> allEmployees;
        public LoginWindowViewModel(IPartnerRepository employeeDataService)
        {
            Employee = new Partner();
            _repo = employeeDataService;
            this.BtnLoginCommand = new Command(this.BtnLoginClick, this.CanCheckCredencials);
            Employee.PropertyChanged += Employee_PropertyChanged;
        }

        public ICommand BtnLoginCommand { get; }
        public Partner Employee { get; set; }
        public event EventHandler LoginCompleted;

        private bool CanCheckCredencials(object arg)
        {
            return !string.IsNullOrEmpty(Employee.UserName);
        }

        private void BtnLoginClick(object obj)
        {
            var currentUser = allEmployees.ToList().Where(c => string.Equals(c.UserName, Employee.UserName, StringComparison.OrdinalIgnoreCase) && string.Equals(c.Password, Employee.Password));

            if (currentUser.FirstOrDefault() == null)
            {
                UIService.ShowMessage("Invalid UserName or Password");
            }
            else
            {
                UIService.CurrentUser = currentUser.FirstOrDefault();
                LoginCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void Load()
        {
            //Task.Run(() =>
            //{
            allEmployees = new List<Partner>(await _repo.GetAllAsync());
            //.ContinueWith((result) =>
             //{
             //    allEmployees = result.Result.ToList();
             //});
            //});            
        }

        private void Employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((Command)this.BtnLoginCommand).RaiseCanExecuteChanged();
        }
    }
}
