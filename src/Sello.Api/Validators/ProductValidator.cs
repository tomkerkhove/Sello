using System.Collections.Generic;
using Sello.Api.Contracts;
using Sello.Api.Resources;
using Sello.Datastore.SQL.Model;

namespace Sello.Api.Validators
{
    public class ProductValidator
    {
        public List<string> Run(ProductInformationContract product, Product persistedProduct)
        {
            var validationMessages = new List<string>();

            if (product == null)
            {
                validationMessages.Add(ValidationMessages.Product_IsRequired);
            }
            else
            {
                if (product.Name != persistedProduct.Name)
                {
                    validationMessages.Add(ValidationMessages.Product_NameIsNotCorrect);
                }
                if (product.Description != persistedProduct.Description)
                {
                    validationMessages.Add(ValidationMessages.Product_DescriptionIsNotCorrect);
                }
                if (product.Price != persistedProduct.Price)
                {
                    validationMessages.Add(ValidationMessages.Product_PriceIsNotCorrect);
                }
            }

            return validationMessages;
        }
    }
}