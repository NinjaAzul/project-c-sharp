using FluentValidation;
using Project_C_Sharp.Common.Constants.Regex;
using Project_C_Sharp.Shared.I18n.Validation.Keys;
using Project_C_Sharp.Shared.Resources.Validation;

namespace Project_C_Sharp.Shared.Validation
{
    public static class ValidationRules
    {
        public static IRuleBuilderOptions<T, string> MustBeValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Nome"))
                .Length(3, 100)
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Nome").Replace("{1}", "3").Replace("{2}", "100"))
                .Matches(RegexPatterns.OnlyLettersAndSpaces)
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Nome_OnlyLetters));
        }

        public static IRuleBuilderOptions<T, string> MustBeValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Senha"))
                .Length(6, 20)
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Senha").Replace("{1}", "6").Replace("{2}", "20"))
                .Matches(RegexPatterns.HasUpperCase)
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Password_RequireUpperCase))
                .Matches(RegexPatterns.HasNumber)
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Password_RequireNumber));
        }

        public static IRuleBuilderOptions<T, string> MustBeValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Email"))
                .EmailAddress()
                .WithMessage(ResourceValidation.GetString(ValidationKeys.Invalid_Email));
        }
    }
}