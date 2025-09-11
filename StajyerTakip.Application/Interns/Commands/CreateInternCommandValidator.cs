using FluentValidation;
using StajyerTakip.Application.Interns.Commands;

namespace StajyerTakip.Application.Interns.Commands;

public sealed class CreateInternCommandValidator : AbstractValidator<CreateInternCommand>
{
    public CreateInternCommandValidator()
    {
        RuleFor(x => x.Entity).NotNull().WithMessage("Model boş olamaz.");

        When(x => x.Entity is not null, () =>
        {
            RuleFor(x => x.Entity!.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur.")
                .MaximumLength(100);

            RuleFor(x => x.Entity!.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur.")
                .MaximumLength(100);

            RuleFor(x => x.Entity!.NationalId)
                .NotEmpty().WithMessage("TC Kimlik zorunludur.")
                .Length(11).WithMessage("TC Kimlik 11 haneli olmalıdır.");

            RuleFor(x => x.Entity!.Email)
                .NotEmpty().WithMessage("Email zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email girin.")
                .MaximumLength(200);

            RuleFor(x => x.Entity!.Status)
                .NotEmpty().WithMessage("Durum zorunludur.");

            RuleFor(x => x.Entity!.EndDate)
                .Must((cmd, end) => end is null || cmd.Entity!.StartDate <= end)
                .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        });
    }
}
