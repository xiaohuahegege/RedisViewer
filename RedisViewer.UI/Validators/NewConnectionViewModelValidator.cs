using FluentValidation;
using RedisViewer.Core;
using RedisViewer.UI.ViewModels;

namespace RedisViewer.UI.Validators
{
    internal class NewConnectionViewModelValidator : AbstractValidator<NewConnectionViewModel>
    {
        public NewConnectionViewModelValidator()
        {
            RuleFor(c => c.Name)
                .Must(c => c != null && c.Trim().Length <= 80)
                .WithMessage("Name");

            RuleFor(c => c.Host)
                .Must(c => c != null && c.Trim().Length <= 40)
                .WithMessage("Host");

            RuleFor(c => c.Port)
                .Must(c => c != null && c.IsPort())
                .WithMessage("Port");
        }
    }
}