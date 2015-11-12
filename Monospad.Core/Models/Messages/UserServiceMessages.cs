using System;
using Monospad.Core.Exceptions;
using TagKid.Framework.Validation;

namespace Monospad.Core.Models.Messages
{
    public class SignupRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UnsavedNoteContent { get; set; }
    }

    public class SigninRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UnsavedNoteContent { get; set; }
    }

    public class SigninWithTokenRequest
    {
        public Guid Token { get; set; }
    }

    public class SignoutRequest
    {
        public Guid Token { get; set; }
    }

    public class RecoverPasswordRequest
    {
        public string Email { get; set; }
    }

    public class SigninRequestValidator : Validator<SigninRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.Email)
                .Email(Errors.Validation_InvalidEmailAddress);

            RuleFor(r => r.Password)
                .NotEmpty(Errors.Validation_InvalidPassword)
                .Length(Errors.Validation_InvalidPassword, 4, 20);
        }
    }

    public class SignupRequestValidator : Validator<SignupRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.Email)
                .Email(Errors.Validation_InvalidEmailAddress);

            RuleFor(r => r.Password)
                .NotEmpty(Errors.Validation_InvalidPassword)
                .Length(Errors.Validation_InvalidPassword, 4, 20);
        }
    }

    public class RecoverPasswordRequestValidator : Validator<RecoverPasswordRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.Email)
                .Email(Errors.Validation_InvalidEmailAddress);
        }
    }
}
