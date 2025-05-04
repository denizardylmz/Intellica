using System;
using System.Linq;

using FluentValidation.Results;

namespace NoteAPI.Tools.Extensions
{
    public static class ValidatorExtensions
    {
        public static string ConcatErrors(this ValidationResult validation)
        {
            if (validation == null)
                throw new ArgumentNullException(nameof(validation));

            if (validation.Errors != null && validation.Errors.Any())
                return String.Join(Environment.NewLine, validation.Errors);
            else
                return String.Empty;
        }
    }
}
