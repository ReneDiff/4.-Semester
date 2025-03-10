using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyBGList.Attributes
{
    public class LettersOnlyAttribute : ValidationAttribute
    {
        public bool UseRegex {get; set;} = false;

        public LettersOnlyAttribute() : base("Value must contain only letters (no spaces, digits, or other chars).") {}

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Value is required.");
            }

            string input = value.ToString()!;

            // Valider baseret på UseRegex-parameteren
            bool isValid = UseRegex ? Regex.IsMatch(input, "^[a-zA-Z]+$") : input.All(char.IsLetter);
            
            return isValid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}