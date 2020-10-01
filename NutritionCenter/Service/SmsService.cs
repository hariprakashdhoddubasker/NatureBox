using NatureBox.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace NatureBox.Service
{
    public class SmsService
    {
        private readonly SMSData mySMS;

        public SmsService()
        {
            mySMS = new SMSData();
        }

        public string SendInvoiceMessage(Customer customer, Invoice invoice)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Dear {customer.Name},");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"Nature Box debited for INR {invoice.Amount}/- Bal INR {customer.BalanceAmount}/-as of {invoice.DateOfPurchase:ddMMMyy hh:mmtt}.");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank You,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Nature Box Nutrition Club");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Eat good Feel good");

            mySMS.Message = stringBuilder.ToString();
            mySMS.MobileNumber = customer.MobileNumber;

            return Send();
        }

        public string SendCustomerPaymentMessage(CustomerPayment customerPayment, List<Partner> admins, string partnerName, string customerName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Dear |Admin|,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"{partnerName} has deposited Rs.{customerPayment.AmountPaid} to {customerName}'s Nature Box account on {customerPayment.DateOfPayment:ddMMMyy hh:mmtt}.");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank You,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Nature Box Nutrition Club");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Eat good Feel good");

            var results = string.Empty;

            foreach (var partner in admins)
            {
                mySMS.Message = stringBuilder.ToString().Replace("|Admin|", partner.UserName);
                mySMS.MobileNumber = partner.MobileNumber;

                if (mySMS.MobileNumber.ToString().Length == 10)
                {
                    results += Send();
                }
            }

            return results;
        }

        public string SendReferralMessage(string Name, long mobileNumber, string message)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Dear Hari,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"App Development referrence");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"Name : {Name}{Environment.NewLine}Mobile No : {mobileNumber}");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"Message : {message}");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank You,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Nature Box");

            mySMS.Message = stringBuilder.ToString();
            mySMS.MobileNumber = 8089947074;

            return Send();
        }

        private string Send()
        {
            string result;

            //New API Link
            //var url = $"http://text.pinger.co.in/index.php/smsapi/httpapi/?uname={mySMS.UserName}&password={mySMS.Password}&sender={mySMS.SenderId}&receiver={mySMS.MobileNumber}&route=TA&msgtype=1&sms={mySMS.Message}";

            //Old API link
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
    }
}
