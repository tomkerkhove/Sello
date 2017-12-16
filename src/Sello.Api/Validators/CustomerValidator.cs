using System.Collections.Generic;
using Sello.Api.Contracts;
using Sello.Api.Resources;

namespace Sello.Api.Validators
{
    public class CustomerValidator
    {
        public List<string> Run(CustomerContract customer)
        {
            var validationMessages = new List<string>();

            if (customer == null)
            {
                validationMessages.Add(ValidationMessages.Customer_IsRequired);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    validationMessages.Add(ValidationMessages.Customer_EmailAddressIsRequired);
                }
                if (string.IsNullOrWhiteSpace(customer.FirstName))
                {
                    validationMessages.Add(ValidationMessages.Customer_FirstNameIsRequired);
                }
                if (string.IsNullOrWhiteSpace(customer.LastName))
                {
                    validationMessages.Add(ValidationMessages.Customer_LastNameIsRequired);
                }
            }

            return validationMessages;
        }
    }
}