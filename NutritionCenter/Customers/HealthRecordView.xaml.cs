using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NatureBox.Customers
{
    /// <summary>
    /// Interaction logic for HealthRecordView.xaml
    /// </summary>
    public partial class HealthRecordView : UserControl
    {
        public HealthRecordView()
        {
            InitializeComponent();
        }
        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                MessageBox.Show("asd");
            }
        }
    }
}
