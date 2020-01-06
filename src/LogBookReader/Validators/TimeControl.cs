using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LogBookReader.Validators
{
    public class TimeControl : ValidationRule
    {
        public int Min { get; set; } = 0;
        public int Max { get; set; } = 0;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string textError = null;
            try
            {
                int.Parse((string)value);
            }
            catch
            {
                textError = $"Значение должно быть двухзначным числом от {Min} до {Max}.";
            }

            if (textError == null)
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, textError);
        }
    }
}
