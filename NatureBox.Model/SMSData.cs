namespace NatureBox.Model
{
    using System;
    using System.Text;

    public class SMSData
    {
        public string UserName { get; } = "naturebox";
        public string Password { get; } = "nature@123";
        public string SenderId { get; set; } = "GOJOTR";
        public long MobileNumber { get; set; }
        public string Message { get; set; }
    }
}
