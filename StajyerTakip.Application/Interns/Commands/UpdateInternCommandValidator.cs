using FluentValidation;
using StajyerTakip.Application.Interns.Commands;

namespace StajyerTakip.Application.Interns.Commands;

public sealed class UpdateInternCommandValidator : AbstractValidator<UpdateInternCommand>
{
    public UpdateInternCommandValidator()
    {
        RuleFor(x => x.Entity).NotNull().WithMessage("Model boş olamaz.");

        When(x => x.Entity is not null, () =>
        {
            RuleFor(x => x.Entity!.Id)
                .GreaterThan(0).WithMessage("Geçersiz Id.");

            RuleFor(x => x.Entity!.FirstName)
                .NotEmpty().MaximumLength(100);

            RuleFor(x => x.Entity!.LastName)
                .NotEmpty().MaximumLength(100);

            RuleFor(x => x.Entity!.NationalId)
                .NotEmpty().Length(11);

            RuleFor(x => x.Entity!.Email)
                .NotEmpty().EmailAddress().MaximumLength(200);

            RuleFor(x => x.Entity!.Status)
                .NotEmpty();

            RuleFor(x => x.Entity!.EndDate)
                .Must((cmd, end) => end is null || cmd.Entity!.StartDate <= end)
                .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        });
    }
}
