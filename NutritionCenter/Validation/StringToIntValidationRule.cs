﻿namespace NatureBox.Validation
{
    using System;
    using System.Windows.Controls;

    public class StringToIntValidationRule : ValidationRule
    {
        public int Length { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (Int64.TryParse(value.ToString(), out _) && value.ToString().Length <= Length)
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Please enter a valid Mobile number.");
        }
    }
}
