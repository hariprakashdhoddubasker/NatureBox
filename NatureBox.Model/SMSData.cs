namespace NatureBox.Model
{
    using System;
    using System.Text;

    public class SMSData
    {
        public string UserName { get; } = "absolute";
        public string Password { get; } = "hari@123";
        public string SenderId { get; set; } = "PINGER";
        public long MobileNumber { get; set; }
        public string Message { get; set; }
    }
}
