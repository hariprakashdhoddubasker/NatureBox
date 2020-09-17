namespace NatureBox.ViewModel
{
    using MaterialDesignThemes.Wpf;
    using NatureBox.Commands;
    using NatureBox.Event;
    using NatureBox.Service;
    using Prism.Events;
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class ItemMenuViewModel : BaseViewModel
    {
        readonly IEventAggregator myEventAggregator;
        public ItemMenuViewModel(string header, List<SubItem> subItems, PackIconKind icon, IEventAggregator eventAggregator)
        {
            myEventAggregator = eventAggregator;
            Header = header;
            SubItems = subItems;
            Icon = icon;
            IsExpanded = false;
            this.OnSelectionChangeCommand = new Command(this.OnSelectionChanged, o => true);
        }

        public ICommand OnSelectionChangeCommand { get; }
        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }

        private bool myIsExpanded;

        public bool IsExpanded
        {
            get
            {
                return myIsExpanded;
            }
            set
            {
                if (string.Equals(this.myIsExpanded, value)) return;
                this.myIsExpanded = value;
                OnPropertyChanged();
            }
        }

        private SubItem mySubItem;

        public SubItem SubItem
        {
            get
            {
                return mySubItem;
            }
            set
            {
                if (string.Equals(this.mySubItem, value)) return;
                this.mySubItem = value;
                OnSelectionChanged(value);
                OnPropertyChanged();
            }
        }

        public UserControl Screen { get; private set; }

        private void OnSelectionChanged(object obj)
        {
            var subItem = (SubItem)obj;
            NatureBoxForms natureBoxForm = Helper.GetNatureBoxForm(subItem.Name);
            myEventAggregator.GetEvent<NavigateViewsEvent>().Publish(natureBoxForm);
        }
    }
}
