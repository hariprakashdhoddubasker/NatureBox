namespace NatureBox.Validation
{
    using System.Windows.Controls;

    public class StringToDoubleValidationRule : ValidationRule
    {
        public int Length { get; set; }
        public string Name { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (double.TryParse(value.ToString(), out _) && value.ToString().Length <= Length)
                return new ValidationResult(true, null);

            return new ValidationResult(false, $"Please enter a valid {Name} number.");
        }
    }
}
