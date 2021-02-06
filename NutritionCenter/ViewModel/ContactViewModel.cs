using NatureBox.Commands;
using NatureBox.Service;
using System.Windows.Input;

namespace NatureBox.ViewModel
{
    public class ContactViewModel : BaseViewModel
    {
        private string myMessage;
        private long myMobileNumber;
        private string myName;

        public ContactViewModel()
        {
            this.BtnSendMessageCommand = new Command(this.OnSendMessageClick, this.CanExecuteSendMessage);
        }

        //public string Name
        //{
        //    get => this.myName;
        //    set
        //    {
        //        SetProperty(ref myName, value);
        //        ((Command)this.BtnSendMessageCommand).RaiseCanExecuteChanged();
        //    }
        //}

        public long MobileNumber
        {
            get => this.myMobileNumber;
            set
            {
                SetProperty(ref myMobileNumber, value);
                ((Command)this.BtnSendMessageCommand).RaiseCanExecuteChanged();
            }
        }

        public string Message
        {
            get => this.myMessage;
            set
            {
                SetProperty(ref myMessage, value);
                ((Command)this.BtnSendMessageCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand BtnSendMessageCommand { get; }

        private bool CanExecuteSendMessage(object arg)
        {
            return long.TryParse(MobileNumber.ToString(), out _) &&
                    MobileNumber.ToString().Length == 10 &&
                    !string.IsNullOrEmpty(Message);
        }

        private void OnSendMessageClick(object obj)
        {
            var smsStatus = new SmsService().SendReferralMessage(MobileNumber, Message);

            //this.Name = string.Empty;
            this.MobileNumber = 0;
            this.Message = string.Empty;
            UIService.ShowMessage(smsStatus);
        }
    }
}
