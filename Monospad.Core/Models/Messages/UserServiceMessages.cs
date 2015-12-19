using System;
using Monospad.Core.Exceptions;
using Taga.Framework.Validation;

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

    public class ResetPasswordRequest
    {
        public Guid Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string NewPassword { get; set; }
    }
    
    public class SigninRequestValidator : Validator<SigninRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.Email)
                .Email(Errors.Validation_InvalidEmailAddress)
                .Length(Errors.Validation_EmailTooLong0.WithArgs(Constants.MaxEmailLength), 1, Constants.MaxEmailLength);

            RuleFor(r => r.Password)
                .NotEmpty(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength))
                .Length(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength), Constants.MinPasswordLength, Constants.MaxPasswordLength);
        }
    }

    public class SignupRequestValidator : Validator<SignupRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.Email)
                .Email(Errors.Validation_InvalidEmailAddress)
                .Length(Errors.Validation_EmailTooLong0.WithArgs(Constants.MaxEmailLength), 1, Constants.MaxEmailLength);

            RuleFor(r => r.Password)
                .NotEmpty(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength))
                .Length(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength), Constants.MinPasswordLength, Constants.MaxPasswordLength);
        }
    }

    public class RecoverPasswordRequestValidator : Validator<RecoverPasswordRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.Email)
                .Email(Errors.Validation_InvalidEmailAddress)
                .Length(Errors.Validation_EmailTooLong0.WithArgs(Constants.MaxEmailLength), 1, Constants.MaxEmailLength);
        }
    }

    public class ResetPasswordRequestValidator : Validator<ResetPasswordRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.NewPassword)
               .NotEmpty(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength))
               .Length(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength), Constants.MinPasswordLength, Constants.MaxPasswordLength);
        }
    }

    public class ChangePasswordRequestValidator : Validator<ChangePasswordRequest>
    {
        protected override void BuildRules()
        {
            RuleFor(r => r.NewPassword)
               .NotEmpty(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength))
               .Length(Errors.Validation_InvalidPasswordLength01.WithArgs(Constants.MinPasswordLength, Constants.MaxPasswordLength), Constants.MinPasswordLength, Constants.MaxPasswordLength);
        }
    }
}
