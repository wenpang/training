using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Training.UnitTests
{
    public class Verification
    {
        public void Validation(ModelStateDictionary modelState, object model)
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            if (model is IValidatableObject validateModel)
                validateModel.Validate(validationContext);

            foreach (ValidationResult error in validationResults)
            {
                modelState.AddModelError(error.MemberNames.Any() ? error.MemberNames.First() : string.Empty, error.ErrorMessage);
            }
        }
    }
}
