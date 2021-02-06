namespace NatureBox.Partners
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            var ctrl = (PasswordBox)sender;
            ((LoginWindowViewModel)this.DataContext).Employee.Password = ctrl.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((LoginWindowViewModel)this.DataContext).Employee.Password = txtPassword.Password;
        }
    }
}
