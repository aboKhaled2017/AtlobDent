using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Utilities.CustomeValidation
{
    public class ValidateCartOrder:ValidationAttribute
    {
        private readonly string _sizeIndexPropertyName;
        private readonly string _productIdPropertyName;
        public ValidateCartOrder(string sizeIndexPropName,string productIdpropName)
        {
            _sizeIndexPropertyName = sizeIndexPropName;
            _productIdPropertyName = productIdpropName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sizeIndexProperty = validationContext.ObjectType.GetProperty(_sizeIndexPropertyName);
            var productIdProperty = validationContext.ObjectType.GetProperty(_productIdPropertyName);
            if(sizeIndexProperty==null)
                return new ValidationResult($"some Property '{_sizeIndexPropertyName}' is undefined.");
            if(productIdProperty == null)
                return new ValidationResult($"some Property '{_productIdPropertyName}' is undefined.");
            var productIdStr = productIdProperty.GetValue(validationContext.ObjectInstance, null);
            var sizeIndex = sizeIndexProperty.GetValue(validationContext.ObjectInstance, null);           
            var productId = Guid.Empty;
            if (!Guid.TryParse(productIdStr.ToString(), out productId))
                return new ValidationResult($"{productIdStr} is not valid id");
            var product = ServiceHelper.GetDbContext().Products.Find(productId);
            if(product==null)
                return new ValidationResult($"the product of id {productId} is not found");
            var _sizeIndex = 0;
            if(!int.TryParse(sizeIndex.ToString(),out _sizeIndex))
                return new ValidationResult($"the sizeIndex {sizeIndex.ToString()} is not valid");
            var sizes = product.sizes.ConvertToListOfStringValues();
            if(sizes.Count-1<_sizeIndex)
                return new ValidationResult($"the sizeIndex {_sizeIndex} was not found");
            return ValidationResult.Success;
        }
    }
}
