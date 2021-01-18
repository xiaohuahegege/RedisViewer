using FluentValidation;
using RedisViewer.UI.ViewModels;

namespace RedisViewer.UI.Validators
{
    internal class NewKeyViewModelValidator : AbstractValidator<NewKeyViewModel>
    {
        public NewKeyViewModelValidator()
        {
            RuleFor(c => c.Name)
                .Must(c => c != null && c.Trim().Length > 0)
                .WithMessage("Name");

            RuleFor(c => c.Value)
                .Must(c => c != null && c.Trim().Length > 0);

            When(c => c.Type == "Zset", () =>
            {
                RuleFor(c => c.Score)
                    .Must(c => c.HasValue)
                    .WithMessage("Score");
            });

            When(c => c.Type == "Hash", () =>
            {
                RuleFor(c => c.Key)
                    .Must(c => c != null && c.Trim().Length > 0)
                    .WithMessage("Key");
            });

            When(c => c.Type == "Stream", () =>
            {
                RuleFor(c => c.Id)
                    .Must(c => c != null && c.Trim().Length > 0)
                    .WithMessage("Id");
            });
        }
    }
}