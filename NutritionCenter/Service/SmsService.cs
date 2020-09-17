using NatureBox.Model;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace NatureBox.Service
{
    public class SmsService
    {
        private readonly Customer myCustomer;
        private readonly Invoice myInvoice;
        private readonly SMSData mySMS;

        public SmsService(Customer customer, Invoice invoice)
        {
            myCustomer = customer;
            myInvoice = invoice;
            mySMS = new SMSData();
            GenerateInvoiceMessage();
        }
        public string Send()
        {
            string result;

            string url = $"http://sms.pinger.co.in/http-api.php?username={mySMS.UserName}&password={mySMS.Password}&senderid={mySMS.SenderId}&route=1&number={mySMS.MobileNumber}&message={mySMS.Message}";

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }

        internal void GenerateInvoiceMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Dear {myCustomer.Name},");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"Nature Box debited for INR {myInvoice.Amount}/- Bal INR {myCustomer.BalanceAmount}/-as of {myInvoice.DateOfPurchase:ddMMMyy hh:mmtt}.");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank You,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Nature Box Nutrition Club");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Eat good Feel good");

            //if (stringBuilder.ToString().Length > 160)
            //{
            //   int nameLimit = 160 - stringBuilder.ToString().Length;
            //    customer.Name.Substring(0, nameLimit);
            //}
            mySMS.Message = stringBuilder.ToString();
            mySMS.MobileNumber = myCustomer.MobileNumber;
        }
    }
}
