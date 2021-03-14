using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PP_02.Validations
{
    class DurationValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "null value");
            int durationSeconds = 0;
            if (!Int32.TryParse(value.ToString(), out durationSeconds))
            {
                return new ValidationResult(false, "Вы ввели не целое число!");
            }
            if (durationSeconds < 0 || durationSeconds > 14400)
            {
                return new ValidationResult(false, "Длительность услуги не может быть дольше 4-х часов или быть отрицательной");
            }

            return ValidationResult.ValidResult;

        }
    }
}
