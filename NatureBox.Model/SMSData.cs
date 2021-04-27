namespace NatureBox.Model
{
    public class SMSData
    {
        public string UserName { get; } = "naturebox";
        public string Password { get; } = "naturebox@123";
        public string Token { get; } = "F3HI6B16";
        public string SenderId { get; set; } = "NATBOX";
        public long MobileNumber { get; set; }
        public string Message { get; set; }
    }
}
