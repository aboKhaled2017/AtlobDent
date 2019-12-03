using Atlob_Dent.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Utilities.CustomeValidation
{
    public class CheckIfUserPropValueIsExixts:ValidationAttribute
    {
        private readonly string _propertyNameToBeChecked;
        private readonly UserPropertyType _userPropertyType;
        private readonly Atlob_dent_Context _Context;
        public CheckIfUserPropValueIsExixts(string propertyNameToBeChecked,UserPropertyType userPropertyType)
        {
            _propertyNameToBeChecked = propertyNameToBeChecked;
            _userPropertyType = userPropertyType;
            _Context = ServiceHelper.GetDbContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var checkedProperty = validationContext.ObjectType.GetProperty(_propertyNameToBeChecked);
            if(checkedProperty == null)
                return new ValidationResult($"some Property '{_propertyNameToBeChecked}' is undefined.");
            var propertyValue = checkedProperty.GetValue(validationContext.ObjectInstance, null);
            if(propertyValue==null)
                return new ValidationResult($"the value of {_propertyNameToBeChecked} property is null");
            string valueStr = propertyValue.ToString();
            switch (_userPropertyType)
            {
                case UserPropertyType.email:
                    { 
                    if(_Context.Users.Any(u=>u.Email== valueStr))
                            return new ValidationResult($"the email {valueStr} is already taken");
                    };
                    break;
                case UserPropertyType.phone:
                    {
                        if (_Context.Users.Any(u => u.PhoneNumber == valueStr))
                            return new ValidationResult($"the phone {valueStr} is already taken");
                    };
                    break;
                case UserPropertyType.userName:
                    {
                        if (_Context.Users.Any(u => u.UserName == valueStr))
                            return new ValidationResult($"the userName {valueStr} is already taken");
                    };
                    break;
                default:
                   return new ValidationResult($"not valid property type"); 
            }
            return ValidationResult.Success;
        }
    }
}
