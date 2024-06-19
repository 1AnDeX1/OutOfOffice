using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Application
{
    public class MyStartDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = (DateTime)value;
            if (startDate < DateTime.Now.Date)
            {
                return new ValidationResult("Start date cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }
}
