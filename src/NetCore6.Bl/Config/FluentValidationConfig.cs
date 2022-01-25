using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore6.Bl.Validations;
using NetCore6.Bl.Validations.User;

namespace NetCore6.Bl.Config
{
    public static class FluentValidationConfig
    {
        public static IMvcBuilder ConfigFluentValidation(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(x =>
            {
                x.AutomaticValidationEnabled = false;
                x.RegisterValidatorsFromAssemblyContaining<ExamplePersonValidator>();
                x.RegisterValidatorsFromAssemblyContaining<UserValidator>();
            });
            return builder;
        }
    }
}