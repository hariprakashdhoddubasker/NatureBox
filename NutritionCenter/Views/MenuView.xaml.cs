namespace NatureBox.Views
{
    using NatureBox.ViewModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for UserControlMenuItem.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView(ItemMenuViewModel itemMenu)
        {
            InitializeComponent();

            ListViewItemMenu.Visibility = Visibility.Collapsed;

            this.DataContext = itemMenu;
        }
    }
}
