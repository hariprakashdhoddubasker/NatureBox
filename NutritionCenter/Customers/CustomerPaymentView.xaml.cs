namespace NatureBox.Customers
{
    using NatureBox.Model;
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class CustomerPaymentView : UserControl
    {
        public List<Customer> Customers;

        public CustomerPaymentView()
        {
            InitializeComponent();
        }
    }
}
