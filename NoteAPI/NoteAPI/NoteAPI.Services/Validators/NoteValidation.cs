using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NoteAPI.Services.Models;

namespace NoteAPI.Services.Validators
{
    public class NoteValidation : AbstractValidator<Note>
    {
        public NoteValidation()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CreatedBy).MaximumLength(100);
            RuleFor(x => x.Tags).MaximumLength(300);
        }
    }   
}
