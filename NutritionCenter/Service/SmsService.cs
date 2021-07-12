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
            stringBuilder.AppendLine($"DEAR {customer.Name},");
            stringBuilder.AppendLine($"Nature Box debited for INR {invoice.Amount}/- Bal INR {customer.BalanceAmount}/- as of ");
            stringBuilder.AppendLine($"{invoice.DateOfPurchase:ddMMMyy hh:mmtt}");
            stringBuilder.AppendLine($"Thank You,");
            stringBuilder.AppendLine($"Nature Box Nutrition Club");
            stringBuilder.AppendLine($"Eat Good Feel Good.");
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

        public string SendReferralMessage(long mobileNumber, string message)
        {
            mySMS.Message = message;
            mySMS.MobileNumber = mobileNumber;

            return Send();
        }

        private string Send()
        {
            string result;
            //Old API link
            //string url = $"http://sms.pinger.co.in/http-api.php?username={mySMS.UserName}&password={mySMS.Password}&senderid={mySMS.SenderId}&route=1&number={mySMS.MobileNumber}&message={mySMS.Message}";

            //New API Link
            //var url = $"http://text.pinger.co.in/index.php/smsapi/httpapi/?uname={mySMS.UserName}&password={mySMS.Password}&sender={mySMS.SenderId}&receiver={mySMS.MobileNumber}&route=TA&msgtype=1&sms={mySMS.Message}";

            //New template registered API link 1
            //var url = $"http://txt.pinger.co.in/vendorsms/pushsms.aspx?user={mySMS.UserName}&password={mySMS.Token}&msisdn={mySMS.MobileNumber}&sid={mySMS.SenderId}&msg={mySMS.Message}&fl=0&gwid=2";

            //"http://txt.pinger.co.in/index.php/smsapi/httpapi/?secret=NK8qAnbJVit1zRuYtxgO&sender=NATBOX&tempid=1207162079173770810&receiver=8089947074&route=TA&msgtype=1&sms=DEAR%20sir,%20Nature%20Box%20debited%20for%20INR%20300%20Bal%20INR%203900%20as%20of%2024%20Thank%20You,%20Nature%20Box%20Nutrition%20Club%20Eat%20Good%20Feel%20Good.
            //New template registered API link 2
            var url = $"http://txt.pinger.co.in/index.php/smsapi/httpapi/?secret={mySMS.SecretCode}&sender={mySMS.SenderId}&receiver={mySMS.MobileNumber}&route=TA&tempid=1207162079173770810&msgtype=1&sms={mySMS.Message}";
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
