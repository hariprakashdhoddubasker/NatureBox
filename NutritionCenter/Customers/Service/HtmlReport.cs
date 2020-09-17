using NatureBox.Model;
using NatureBox.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NatureBox.Customers.Service
{
    public class HtmlReport
    {
        public static string Generate(List<HealthRecord> healthRecords, Customer customer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Constants.HtmlStartTag);

            stringBuilder.Append(Constants.HeadStartTag);
            stringBuilder.Append(Constants.StyleTag);
            stringBuilder.Append(Constants.HeadEndTag);

            stringBuilder.Append(Constants.BodyStartTag);

            stringBuilder.AppendFormat(Constants.H1Tag, "Nature Box Health Report");
            stringBuilder.Append(Constants.Break);
            stringBuilder.Append(Constants.Break);

            stringBuilder.Append("<div id=\"background\"><p id=\"bg-text\">Nature Box</p></div>");
            stringBuilder.Append("<div id=\"content\">");

            stringBuilder.AppendFormat(Constants.TableStartTag, "width=\"40%\"");
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.BGTableDataTag, "Customer Details");
            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.TableAlternativeDataTag, "Name");
            stringBuilder.AppendFormat(Constants.TableDataTag, customer.Name);
            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.TableAlternativeDataTag, "Mobile Number");
            stringBuilder.AppendFormat(Constants.TableDataTag, customer.MobileNumber);
            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.TableAlternativeDataTag, "Customer Id");
            stringBuilder.AppendFormat(Constants.TableDataTag, customer.CustomerId);
            stringBuilder.Append(Constants.TableRowEndTag);
            stringBuilder.Append(Constants.TableEndTag);
            stringBuilder.Append(Constants.Break);
            stringBuilder.Append(Constants.Break);

            stringBuilder.AppendFormat(Constants.TableStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 5, "Date");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 5, "Weight");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 5, "Chest");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 5, "Waist");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "Hip");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "BMI");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "BMR");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "Fat");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "V Fat");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "Bone Mass");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "Muscle Mass");
            stringBuilder.AppendFormat(Constants.TableHeaderTag, 10, "Water");
            stringBuilder.AppendFormat(Constants.TableRowStartTag, " class=\"header\"");
            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
            foreach (var healthRecord in healthRecords)
            {
                var tablecellTag = Constants.TableDataTag;
                stringBuilder.AppendFormat(Constants.TableRowStartTag, $" title=\"{healthRecord.BMI}\"");
                stringBuilder.AppendFormat(tablecellTag, healthRecord.RecordedDateString);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.Weight);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.Chest);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.Waist);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.Hip);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.BMI);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.BMR);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.Fat);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.VFat);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.BoneMass);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.MuscleMass);
                stringBuilder.AppendFormat(tablecellTag, healthRecord.Waist);
                stringBuilder.Append(Constants.TableRowEndTag);
            }

            stringBuilder.Append(Constants.TableEndTag);
            stringBuilder.Append("</div>");
            stringBuilder.Append(Constants.BodyEndTag);
            stringBuilder.Append(Constants.HtmlEndTag);

            string drivePath = Helper.GetAvailableDrivePath();
            var folderPath = drivePath + "Reports";
            Directory.CreateDirectory(folderPath);

            var fileSavePath = $"{ folderPath }\\{customer.Name}_{customer.CustomerId}_{DateTime.Now:ddMMMyyyy}.html";

            using (FileStream fs = new FileStream(fileSavePath, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(stringBuilder.ToString());
                }
            }

            return fileSavePath;
        }
    }
}
