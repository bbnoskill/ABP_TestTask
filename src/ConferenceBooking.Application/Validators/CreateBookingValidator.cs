using FluentValidation;
using ConferenceBooking.Application.DTOs.Booking;

namespace ConferenceBooking.Application.Validators
{
    /// <summary>Валідатор створення бронювання.</summary>
    public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("ID залу обов'язковий.");

            RuleFor(x => x.StartDateTime)
                .NotEmpty().WithMessage("Час початку обов'язковий.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Бронювання можливе лише на майбутній час.");

            RuleFor(x => x.EndDateTime)
                .NotEmpty().WithMessage("Час завершення обов'язковий.")
                .GreaterThan(x => x.StartDateTime)
                    .WithMessage("Час завершення повинен бути пізніше часу початку.");

            RuleFor(x => x)
                .Must(x => (x.EndDateTime - x.StartDateTime).TotalMinutes >= 30)
                .WithMessage("Мінімальна тривалість бронювання — 30 хвилин.")
                .When(x => x.EndDateTime > x.StartDateTime);
        }
    }
}
