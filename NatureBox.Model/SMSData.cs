namespace NatureBox.Model
{
    public class SMSData
    {
        public string UserName { get; } = "natureboxnutrition";
        public string Password { get; } = "naturebox@123";
        public string SenderId { get; set; } = "NATBOX";
        public long MobileNumber { get; set; }
        public string Message { get; set; }
    }
}
