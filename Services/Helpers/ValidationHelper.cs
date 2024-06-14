using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            //Model Validations 
            ValidationContext validationContext = new ValidationContext(obj);
            //for storing the errors 
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true); //if u fail to supply true then it will supplu only required parameters to validate
            //convert personAddRequest object into Person Type
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
