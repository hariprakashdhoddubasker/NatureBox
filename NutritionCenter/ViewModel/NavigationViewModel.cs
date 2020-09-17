namespace NatureBox.ViewModel
{
    using MaterialDesignThemes.Wpf;
    using NatureBox.Service;
    using NatureBox.Views;
    using Prism.Events;
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    public class NavigationViewModel : BaseViewModel
    {
        private StackPanel myMenu;
        IEventAggregator myEventAggregator;
        public NavigationViewModel(IEventAggregator eventAggregator)
        {
            myEventAggregator = eventAggregator;
            MenuItem = GetMenuItems();
        }

        public StackPanel MenuItem
        {
            get
            {
                return myMenu;
            }
            set
            {
                if (string.Equals(this.myMenu, value)) return;
                this.myMenu = value;
                OnPropertyChanged();
            }
        }

        internal StackPanel GetMenuItems()
        {
            var registorSubItems = new List<SubItem>
            {
                new SubItem(NatureBoxForms.Customer.GetDescription()),
                new SubItem(NatureBoxForms.HealthRecord.GetDescription())
            };

            var reportSubItems = new List<SubItem>
            {
                new SubItem(NatureBoxForms.CustomerInvoiceReport.GetDescription()),
                new SubItem(NatureBoxForms.PartnerInvoiceReport.GetDescription()),
                new SubItem(NatureBoxForms.CustomerPaymentReport.GetDescription())
            };

            if (UIService.CurrentUser.Role == NatureBoxRoles.Admin.ToString())
            {
                registorSubItems.Add(new SubItem(NatureBoxForms.Partner.GetDescription()));
                registorSubItems.Add(new SubItem(NatureBoxForms.Product.GetDescription()));

                reportSubItems.Add(new SubItem(NatureBoxForms.ParnterSettlementReport.GetDescription()));
                reportSubItems.Add(new SubItem(NatureBoxForms.BackUp.GetDescription()));
            }

            var financialSubItems = new List<SubItem>
            {
                new SubItem(NatureBoxForms.CustomerPayment.GetDescription()),
                new SubItem(NatureBoxForms.ParterSettlement.GetDescription()),
                new SubItem(NatureBoxForms.Invoice.GetDescription())
            };

            var financialItemMenu = new ItemMenuViewModel("FINANCIAL", financialSubItems, PackIconKind.ScaleBalance, myEventAggregator);
            var registorItemMenu = new ItemMenuViewModel("REGISTER", registorSubItems, PackIconKind.Register, myEventAggregator);
            var reportItemMenu = new ItemMenuViewModel("REPORTS", reportSubItems, PackIconKind.FileReport, myEventAggregator);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(new MenuView(registorItemMenu));
            stackPanel.Children.Add(new MenuView(financialItemMenu));
            stackPanel.Children.Add(new MenuView(reportItemMenu));
            return stackPanel;
        }
    }
}
