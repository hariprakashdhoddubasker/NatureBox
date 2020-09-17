using System;
using System.IO;

namespace NatureBox.Service
{
    public class Helper
    {
        public static NatureBoxForms GetNatureBoxForm(string formName)
        {
            NatureBoxForms natureBoxForms;
            foreach (string name in Enum.GetNames(typeof(NatureBoxForms)))
            {
                natureBoxForms = (NatureBoxForms)Enum.Parse(typeof(NatureBoxForms), name);
                if (formName == natureBoxForms.GetDescription())
                {
                    return natureBoxForms;
                }
            }
            return NatureBoxForms.Invoice;
        }

        public static string GetAvailableDrivePath()
        {
            var drivePath = string.Empty;
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.Name != @"C:\")
                {
                    drivePath = drive.Name;
                    break;
                }
            }
            var natureBoxPath = drivePath + "\\NatureBox\\";
            Directory.CreateDirectory(drivePath + "\\NatureBox\\");
            return natureBoxPath;
        }
    }
}
