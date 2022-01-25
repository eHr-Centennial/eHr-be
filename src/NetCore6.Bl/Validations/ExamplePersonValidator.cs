using FluentValidation;
using NetCore6.Bl.DTOs;

namespace NetCore6.Bl.Validations
{
    public class ExamplePersonValidator: AbstractValidator<ExamplePersonDTO>
    {
        public ExamplePersonValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is required");
        }
    }
}