using System.ComponentModel.DataAnnotations;

namespace Eventive.Models.Validations
{
    public class EnforceTrueAttribute : ValidationAttribute
    {
        public EnforceTrueAttribute() : base("This field must be valid.") { }

        public override bool IsValid(object value) => value is bool valueAsBool && valueAsBool;
    }
}
